using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _Context;
        private readonly AppSettings _AppSettings;
        public UserController(MyDbContext Context,IOptions<AppSettings> AppSettings)
        {
            _Context = Context;
            _AppSettings = AppSettings.Value;
        }
        [HttpPost("Login")]
        public IActionResult Validate(LoginModel model)
        {
            var user = _Context.Users.SingleOrDefault(u=>u.UserName == model.UserName && u.Password == model.Password);
            if(user==null)
            {
                return Ok(new ApiResponse{
                   Success = false,
                   Message ="Invalid username or password" 
                });
            }else
            {
                // Provide Token 
                return Ok(new ApiResponse{
                    Success = true,
                    Message = "Authentication success",
                    Data = GenerateToken(user)
                });
            }
            return Ok();

        }
        private string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_AppSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim("UserName",user.UserName),
                    new Claim("Id",user.Id.ToString()),
                    new Claim("TokenId",Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                (secretKeyBytes),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}