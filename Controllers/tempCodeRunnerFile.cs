//  [HttpPost("RenewToken")]
//         public async Task<IActionResult> RenewToken(TokenModel model)
//         {
//             var jwtHandler = new JwtSecurityTokenHandler();
//             var secretKeyBytes = Encoding.UTF8.GetBytes(_AppSettings.SecretKey);
//             var tokenValidateParam = new TokenValidationParameters
//             {
//                 // Tu sinh token
//                 ValidateIssuer = false,
//                 ValidateAudience = false,
//                 // Ky vao token
//                 ValidateIssuerSigningKey = true,
//                 IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
//                 ClockSkew = TimeSpan.Zero,
//                 ValidateLifetime = false // Khong kiem tra expire cua token
//             };
//             try
//             {
//                 // Check 1 : AccessToken valid format
//                 var tokenInVerification = jwtHandler.ValidateToken(model.AccessToken,tokenValidateParam,out var validatedToken);
//                 // Check 2 : Check algorithm
//                 if(validatedToken is JwtSecurityToken jwtSecurityToken)
//                 {
//                     var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);
//                     if(!result){
//                         return Ok(new ApiResponse{
//                             Success = false,
//                             Message = "Invalid token"
//                         });
//                     }
//                 }
//                 // Check 3 : Check accessToken expire?
//                 var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>
//                                     x.Type==JwtRegisteredClaimNames.Exp).Value);
//                 var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
//                 if(expireDate>DateTime.UtcNow)
//                 {
//                     return Ok(new ApiResponse{
//                     Success = true,
//                     Message = "Access Token has not yet expired"
//                     }); 
//                 }
//                 // Check 4 : Check refreshToken existed in DB
//                 var storedToken =_Context.RefreshTokens.FirstOrDefault(x=>
//                                 x.Token == model.RefreshToken);
//                 if(storedToken==null)
//                 {
//                     return Ok(new ApiResponse{
//                     Success = true,
//                     Message = "Access Token does not exist"
//                     }); 
//                 }
//                 // Check 5 : check refreshToken is used/revoked ?
//                 if(storedToken.IsUsed)
//                 {
//                     return Ok(new ApiResponse{
//                     Success = true,
//                     Message = "Access Token has been used"
//                     }); 
//                 }
//                 if(storedToken.IsRevoked)
//                 {
//                     return Ok(new ApiResponse{
//                     Success = true,
//                     Message = "Access Token has been revoked"
//                     }); 
//                 }
//                 // Check 6 : Access Token id == JwtId in refreshToken
//                 var jti  = tokenInVerification.Claims.FirstOrDefault(x=>
//                         x.Type == JwtRegisteredClaimNames.Jti).Value;
//                 if(storedToken.JwtId!=jti)
//                 {
//                    return Ok(new ApiResponse{
//                     Success = true,
//                     Message = "Token does not match"
//                     });  
//                 }
//                 // Check 7 : Token is used
//                 storedToken.IsRevoked = true;
//                 storedToken.IsUsed = true;
//                 _Context.Update(storedToken);
//                 await _Context.SaveChangesAsync();
//                 // Create new Token
//                 var user = _Context.Users.FirstOrDefault(u=>u.Id == storedToken.UserId);
//                 var token = await GenerateToken(user);
//                 return Ok(new ApiResponse{
//                     Success = true,
//                     Message = "Renew token sucess",
//                     Data = token
//                 });
//             }
//             catch (System.Exception)
//             {
//                 return BadRequest(new ApiResponse{
//                         Success = false,
//                         Message = "Something went wrong"
//                     });
//             }
//         }