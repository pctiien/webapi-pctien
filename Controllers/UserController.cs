using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserRepository _IUserRepository{set;get;}
        public UserController(IUserRepository IUserRepository)
        {
            _IUserRepository = IUserRepository;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Validate(LoginModel model)
        {
            return Ok(await _IUserRepository.Validate(model));
        }
        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            return Ok(await _IUserRepository.RenewToken(model));
        }
    }
}