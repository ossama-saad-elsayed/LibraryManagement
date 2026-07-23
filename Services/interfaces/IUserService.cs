using LibraryManagement.DTOS;
using static LibraryManagement.DTOS.CreateUserDto;

namespace LibraryManagement.Services.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(RegisterUserDto request);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto request);
        Task<bool> DeleteUserAsync(int id);
    }
}
