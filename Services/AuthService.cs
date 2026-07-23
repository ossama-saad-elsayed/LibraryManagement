using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagement.Services
{
    public class AuthService:IAuthService
    {

        private readonly LibraryManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(LibraryManagementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginDTO request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
                return null;
           
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password , user.PasswordHash);
            if (!isValidPassword)
                return null; 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) 
            };

         
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "THIS_IS_A_VERY_SECRET_KEY_123456"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "LibraryApi",
                audience: _configuration["Jwt:Audience"] ?? "LibraryApiUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

