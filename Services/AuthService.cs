using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly LibraryManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(LibraryManagementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDTO request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
                return null;

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
                return null;

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var candidates = await _context.RefreshTokens
                .Where(rt => !rt.IsUsed && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            var storedRefreshToken = candidates.FirstOrDefault(rt =>
                BCrypt.Net.BCrypt.Verify(request.RefreshToken, rt.TokenHash));

            if (storedRefreshToken == null)
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(request.AccessToken);
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (jti != storedRefreshToken.JwtId)
                return null;

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);

            var user = await _context.Users.FindAsync(storedRefreshToken.UserId);
            if (user == null)
                return null;

            return await GenerateAuthResponseAsync(user);
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var candidates = await _context.RefreshTokens
                .Where(rt => !rt.IsUsed && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            var storedToken = candidates.FirstOrDefault(rt =>
                BCrypt.Net.BCrypt.Verify(refreshToken, rt.TokenHash));

            if (storedToken == null)
                return false;

            storedToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<LoginResponseDto> GenerateAuthResponseAsync(User user)
        {
            var jwtId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, jwtId),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "LibraryApi",
                audience: _configuration["Jwt:Audience"] ?? "LibraryApiUsers",
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshTokenString = GenerateRefreshToken();
            var refreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshTokenString);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                JwtId = jwtId,
                IsUsed = false,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshTokenString,
                ExpiresAt = expiresAt
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
