using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class UserRepository : IUserRepository
    {
        public AppSettings _AppSettings{set;get;}
        private readonly MyDbContext _Context;
        public UserRepository(MyDbContext context,IOptionsMonitor<AppSettings> AppSettings)
        {
            _AppSettings = AppSettings.CurrentValue;
            _Context = context;
        }
        public async Task<TokenModel> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_AppSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserName",user.UserName),
                    new Claim("Id",user.Id.ToString()),
                    new Claim("TokenId",Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                (secretKeyBytes),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken  = jwtTokenHandler.WriteToken(token);
            var refreshToken =GenerateRefreshToken();
           // Luu vao database
            var refreshTokenEf = new RefreshToken{
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                Token = refreshToken,
                IsUsed = false,
                UserId= user.Id,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };
            await _Context.RefreshTokens.AddAsync(refreshTokenEf);
            await _Context.SaveChangesAsync();
            return new TokenModel{
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };        
        }

        public string GenerateRefreshToken()
            { 
                var random = new byte[32];
                using(var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(random);
                    return Convert.ToBase64String(random);
                }
            }

        public async Task<ApiResponse> RenewToken(TokenModel model)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_AppSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                // Tu sinh token
                ValidateIssuer = false,
                ValidateAudience = false,
                // Ky vao token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false // Khong kiem tra expire cua token
            };
            try
            {
                // Check 1 : AccessToken valid format
                var tokenInVerification = jwtHandler.ValidateToken(model.AccessToken,tokenValidateParam,out var validatedToken);
                // Check 2 : Check algorithm
                if(validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);
                    if(!result){
                        return new ApiResponse{
                            Success = false,
                            Message = "Invalid token"
                        };
                    }
                }
                // Check 3 : Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>
                                    x.Type==JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixDateToDateTime(utcExpireDate);
                if(expireDate>DateTime.UtcNow)
                {
                    return new ApiResponse{
                    Success = true,
                    Message = "Access Token has not yet expired"
                    }; 
                }
                // Check 4 : Check refreshToken existed in DB?
                var storedToken =_Context.RefreshTokens.FirstOrDefault(x=>
                                x.Token == model.RefreshToken);
                if(storedToken==null)
                {
                    return new ApiResponse{
                    Success = true,
                    Message = "Access Token does not exist"
                    }; 
                }
                // Check 5 : check refreshToken is used/revoked ?
                if(storedToken.IsUsed)
                {
                    return new ApiResponse{
                    Success = true,
                    Message = "Access Token has been used"
                    }; 
                }
                if(storedToken.IsRevoked)
                {
                    return new ApiResponse{
                    Success = true,
                    Message = "Access Token has been revoked"
                    }; 
                }
                // Check 6 : Access Token id == JwtId in refreshToken
                var jti  = tokenInVerification.Claims.FirstOrDefault(x=>
                        x.Type == JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.JwtId!=jti)
                {
                   return new ApiResponse{
                    Success = true,
                    Message = "Token does not match"
                    };  
                }
                // Check 7 : Token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _Context.Update(storedToken);
                await _Context.SaveChangesAsync();
                // Create new Token
                var user = _Context.Users.FirstOrDefault(u=>u.Id == storedToken.UserId);
                var token = await GenerateToken(user);
                return new ApiResponse{
                    Success = true,
                    Message = "Renew token sucess",
                    Data = token
                };
            }
            catch (System.Exception)
            {
                return new ApiResponse{
                        Success = false,
                        Message = "Something went wrong"
                    };
            }
        }

        public async Task<ApiResponse> Validate(LoginModel model)
        {
            var user = _Context.Users.SingleOrDefault(u=>u.UserName == model.UserName && u.Password == model.Password);
            if(user==null)
            {
                return new ApiResponse{
                   Success = false,
                   Message ="Invalid username or password" 
                };
            }else
            {
              // Provide Token 
                var token =  await GenerateToken(user);
                return new ApiResponse{
                    Success = true,
                    Message = "Authentication success",
                    Data =token
                };
            }
        }

        public DateTime ConvertUnixDateToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate);
            return dateTimeInterval;
        }

    }
}