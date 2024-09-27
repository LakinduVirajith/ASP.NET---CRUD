using ASP.NET___CRUD.Models;
using Microsoft.AspNetCore.Identity;

namespace ASP.NET___CRUD.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<(string token, string refreshToken)> AuthenticateUserAsync(LoginModel model);

        Task<(string token, string refreshToken)> RefreshTokenAsync(string refreshToken);
    }
}
