using ASP.NET___CRUD.Models;
using Microsoft.AspNetCore.Identity;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASP.NET___CRUD.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // Register a new user and assign a role
        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            // Check if the email is already registered
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                // Handle the case where the email is already registered
                return IdentityResult.Failed(new IdentityError { Description = "Email is already in use." });
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && !string.IsNullOrWhiteSpace(model.Role))
            {
                // Assign the role to the user if it's provided
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return result;
        }

        // Authenticate a user and return a JWT token
        public async Task<(string token, string refreshToken)> AuthenticateUserAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            // Check if the user exists and the password is correct
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateToken(user, roles);
                var refreshToken = _tokenService.GenerateRefreshToken(user);

                return (token, refreshToken);
            }

            // Return null if authentication fails
            return (null, null);
        }

        // Validate the refresh token and generate new tokens
        public async Task<(string token, string refreshToken)> RefreshTokenAsync(string refreshToken)
        {
            // Validate the refresh token and check its expiration
            var user = await ValidateRefreshToken(refreshToken);
            if (user == null)
            {
                return (null, null); // Invalid refresh token
            }

            // Generate new tokens
            var roles = await _userManager.GetRolesAsync(user);
            var newToken = _tokenService.GenerateToken(user, roles);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user); // Create a new refresh token

            // Optionally: Store the new refresh token in your database or cache

            return (newToken, newRefreshToken);
        }

        private async Task<ApplicationUser?> ValidateRefreshToken(string refreshToken)
        {
            // Example of decoding the token to check expiration
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(refreshToken);
                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    // Token has expired
                    return null;
                }

                // Retrieve the user's email from the token claims
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim == null) return null;

                // Find the user based on the email
                var userEmail = emailClaim.Value;
                return await _userManager.FindByEmailAsync(userEmail);
            }
            catch
            {
                // Handle token validation exceptions (e.g., invalid token)
                return null;
            }
        }
    }
}
