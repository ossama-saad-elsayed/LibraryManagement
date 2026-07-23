using LibraryManagement.DTOS;

namespace LibraryManagement.Services.interfaces
{
    public interface IBorrowRecordService
    {
        Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync();
        Task<BorrowRecordDto?> GetBorrowRecordByIdAsync(int id);
        Task<IEnumerable<BorrowRecordDto>> GetBorrowRecordsByMemberIdAsync(int memberId);
        Task<BorrowRecordDto> BorrowBookAsync(CreateBorrowRecordDto request);
        Task<BorrowRecordDto?> ReturnBookAsync(int id);
    }
}
