using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IFineService
    {
        Task<IEnumerable<FineDto>> GetAllFinesAsync();
        Task<FineDto?> GetFineByIdAsync(int id);
        Task<IEnumerable<FineDto>> GetFinesByMemberIdAsync(int memberId);
        Task<FineDto?> PayFineAsync(int fineId);
        Task<FineDto> CreateFineAsync(CreateFineDto request);
        Task<bool> DeleteFineAsync(int id);

    }
}
