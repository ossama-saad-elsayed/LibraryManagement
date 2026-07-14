using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Nullable because it might not be returned yet
        public string Status { get; set; } = null!;

        public string BookTitle { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }

    public class CreateBorrowRecordDto
    {
        [Required(ErrorMessage = "BookId is required.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Borrow date is required.")]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }
    }
}