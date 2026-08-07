using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDTO request);
        Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<bool> LogoutAsync(string refreshToken);
    }
}
