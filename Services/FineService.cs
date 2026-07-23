using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class FineService : IFineService
    {
        private readonly LibraryManagementDbContext _context;

        public FineService(LibraryManagementDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FineDto>> GetAllFinesAsync()
        {
            var fines = await _context.Fines
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Book)
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Member)
                .ToListAsync();

            return fines.Select(MapToDto);
        }

        public async Task<FineDto?> GetFineByIdAsync(int id)
        {
            var fine = await _context.Fines
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Book)
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Member)
                .FirstOrDefaultAsync(f => f.Id == id);

            return fine == null ? null : MapToDto(fine);
        }

        public async Task<IEnumerable<FineDto>> GetFinesByMemberIdAsync(int memberId)
        {
            var fines = await _context.Fines
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Book)
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Member)
                .Where(f => f.BorrowRecord.MemberId == memberId)
                .ToListAsync();

            return fines.Select(MapToDto);
        }

        public async Task<FineDto?> PayFineAsync(int fineId)
        {
            var fine = await _context.Fines
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Book)
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Member)
                .FirstOrDefaultAsync(f => f.Id == fineId);

            if (fine == null) return null;

            fine.IsPaid = true;
            fine.PaidDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(fine);
        }

        public async Task<FineDto> CreateFineAsync(CreateFineDto request)
        {
            var borrowRecord = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == request.BorrowRecordId);

            if (borrowRecord == null)
            {
                throw new KeyNotFoundException($"BorrowRecord with ID {request.BorrowRecordId} was not found.");
            }

            var fine = new Fine
            {
                BorrowRecordId = request.BorrowRecordId,
                Amount = request.Amount,
                IsPaid = false,
                PaidDate = null
            };

            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();

            fine.BorrowRecord = borrowRecord;
            return MapToDto(fine);
        }

        private static FineDto MapToDto(Fine f)
        {
            return new FineDto
            {
                Id = f.Id,
                BorrowRecordId = f.BorrowRecordId,
                MemberId = f.BorrowRecord?.MemberId ?? 0,
                MemberFullName = f.BorrowRecord?.Member?.FullName ?? string.Empty,
                BookTitle = f.BorrowRecord?.Book?.Title ?? string.Empty,
                Amount = f.Amount,
                IsPaid = f.IsPaid,
                PaidDate = f.PaidDate
            };
        }
    }
}
