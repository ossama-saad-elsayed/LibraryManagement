using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(RegisterUserDto request);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto request);
        Task<bool> DeleteUserAsync(int id);
    }
}
