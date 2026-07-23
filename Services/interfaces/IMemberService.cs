using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllMembersAsync();
        Task<MemberDto?> GetMemberByIdAsync(int id);
        Task<MemberDto> CreateMemberAsync(CreateMemberDto request);
        Task<MemberDto?> UpdateMemberAsync(int id, UpdateMemberDto request);
        Task<bool> DeleteMemberAsync(int id);
    }
}
