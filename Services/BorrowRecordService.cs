using LibraryManagement.DTOS;
using LibraryManagement.Entities;
using LibraryManagement.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LibraryManagement.Services
{
    public class BorrowRecordService : IBorrowRecordService
    {
        private readonly LibraryManagementDbContext _context;
        private readonly FineOptions _fine;

        public BorrowRecordService(LibraryManagementDbContext context, IOptions<FineOptions> fineOptions)
        {
            _context = context;
            _fine = fineOptions.Value;
        }
        public async Task<IEnumerable<BorrowRecordDto>> GetAllBorrowRecordsAsync()
        {
            var records = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .ToListAsync();

            return records.Select(MapToDto);
        }

        public async Task<BorrowRecordDto?> GetBorrowRecordByIdAsync(int id)
        {
            var record = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);

            return record == null ? null : MapToDto(record);
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetBorrowRecordsByMemberIdAsync(int memberId)
        {
            var records = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .Where(b => b.MemberId == memberId)
                .ToListAsync();

            return records.Select(MapToDto);
        }

        public async Task<BorrowRecordDto> BorrowBookAsync(CreateBorrowRecordDto request)
        {
            var member = await _context.Members.FindAsync(request.MemberId);
            if (member == null)
            {
                throw new KeyNotFoundException($"Member with ID {request.MemberId} was not found.");
            }

            if (_fine.BlockBorrowingOnUnpaidFines)
            {
                var hasUnpaidFines = await _context.Fines
                    .AnyAsync(f => !f.IsPaid && f.BorrowRecord.MemberId == request.MemberId);
                if (hasUnpaidFines)
                    throw new InvalidOperationException(
                        $"Member with ID {request.MemberId} has unpaid fines and cannot borrow.");
            }
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {request.BookId} was not found.");
            }

            if (book.AvailableCopies <= 0)
            {
                throw new InvalidOperationException($"Book '{book.Title}' is currently unavailable for borrowing.");
            }

            book.AvailableCopies--;

            var record = new BorrowRecord
            {
                BookId = request.BookId,
                MemberId = request.MemberId,
                BorrowDate = request.BorrowDate,
                DueDate = request.DueDate,
                Status = "Borrowed"
            };

            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();

            record.Book = book;
            record.Member = member;

            return MapToDto(record);
        }

        public async Task<BorrowRecordDto?> ReturnBookAsync(int id)
        {
            var record = await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (record == null) return null;

            if (record.Status == "Returned")
            {
                return MapToDto(record);
            }

            var returnDate = DateTime.UtcNow;
            record.ReturnDate = returnDate;
            record.Status = "Returned";

            if (record.Book != null && record.Book.AvailableCopies < record.Book.CopiesOwned)
            {
                record.Book.AvailableCopies++;
            }
            var fineAmount = CalculateOverdueAmount(record.DueDate, returnDate, _fine);
            if (fineAmount.HasValue)
            {
                _context.Fines.Add(new Fine
                {
                    BorrowRecordId = record.Id,
                    Amount = fineAmount.Value,
                    IsPaid = false,
                    PaidDate = null
                });
            }
            await _context.SaveChangesAsync();
            return MapToDto(record);
        }
            

            
        

        private static BorrowRecordDto MapToDto(BorrowRecord b)
        {
            return new BorrowRecordDto
            {
                Id = b.Id,
                BookId = b.BookId,
                MemberId = b.MemberId,
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                ReturnDate = b.ReturnDate,
                Status = b.Status,
                BookTitle = b.Book?.Title ?? string.Empty,
                MemberFullName = b.Member?.FullName ?? string.Empty
            };
        }

        public static decimal? CalculateOverdueAmount(DateTime dueDate, DateTime asOfDate, FineOptions s)
        {
            var days = (int)Math.Ceiling((asOfDate - dueDate).TotalDays) - s.GracePeriodDays;
            return days > 0 ? Math.Min(days * s.DailyFineRate, s.MaxFineCap) : null;
        }
    }
}
