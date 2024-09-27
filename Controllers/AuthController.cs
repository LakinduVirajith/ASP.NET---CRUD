using ASP.NET___CRUD.Helpers;
using ASP.NET___CRUD.Models;
using ASP.NET___CRUD.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET___CRUD.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/v1/auth/sign-up
        [HttpPost("sign-up")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                return Ok("User registered successfully!");
            }

            return BadRequest(result.Errors);
        }

        // POST: api/v1/auth/sign-in
        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var (token, refreshToken) = await _authService.AuthenticateUserAsync(model);

            if (token != null)
            {
                return Ok(new { Token = token, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }

        // POST: api/v1/auth/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            // Validate the refresh token and generate new tokens
            var (newToken, newRefreshToken) = await _authService.RefreshTokenAsync(refreshToken);
            if (newToken != null)
            {
                return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
            }

            return Unauthorized();
        }
    }
}
