using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public string Status { get; set; } = null!;

        public string BookTitle { get; set; } = null!;
        public string MemberFullName { get; set; } = null!;
    }

    public class CreateBorrowRecordDto
    {
        [Required(ErrorMessage = "BookId is required.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "MemberId is required.")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Borrow date is required.")]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }
    }
}