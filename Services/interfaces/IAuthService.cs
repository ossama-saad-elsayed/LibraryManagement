using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginDTO request);
    }
}
