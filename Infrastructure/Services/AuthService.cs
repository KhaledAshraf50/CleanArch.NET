using Application.Common.Interfaces;
using Application.Features.Auth.Common;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext dbContext,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(";", result.Errors.Select(e => e.Description)));
            }

            return await GenerateTokensForUserAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) throw new InvalidOperationException("Invalid credentials");

            var sign = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!sign.Succeeded) throw new InvalidOperationException("Invalid credentials");

            return await GenerateTokensForUserAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken && t.Revoked == null && t.Expires > DateTime.UtcNow);
            if (token == null) throw new InvalidOperationException("Invalid refresh token");

            var user = await _userManager.FindByIdAsync(token.UserId);
            if (user == null) throw new InvalidOperationException("Invalid refresh token user");

            // revoke old token
            token.Revoked = DateTime.UtcNow;
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync();

            return await GenerateTokensForUserAsync(user);
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken && t.Revoked == null);
            if (token == null) return false;
            token.Revoked = DateTime.UtcNow;
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task<AuthResponseDto> GenerateTokensForUserAsync(ApplicationUser user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expireMinutes = jwtSection.GetValue<int>("ExpireMinutes");
            var refreshExpireDays = jwtSection.GetValue<int>("RefreshExpireDays");

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new Domain.Entities.RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.AddDays(refreshExpireDays),
                UserId = user.Id
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = expires
            };
        }
    }
}
