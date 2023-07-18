using api.Data;
using api.Models;

namespace api.Services
{
    public interface IUserRepository 
    {
        Task<ApiResponse> Validate(LoginModel model);
        Task<ApiResponse> RenewToken(TokenModel model);
        Task<TokenModel> GenerateToken(User user);
        String GenerateRefreshToken();
        DateTime ConvertUnixDateToDateTime(long utcExpireDate);
    }
}