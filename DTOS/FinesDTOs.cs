using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class FineDto
    {
        public int Id { get; set; }
        public int BorrowRecordId { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; } // Nullable until paid

        public string UserName { get; set; } = null!;
        public string BookTitle { get; set; } = null!;
    }

    public class CreateFineDto
    {
        [Required(ErrorMessage = "BorrowRecordId is required.")]
        public int BorrowRecordId { get; set; }

        [Required(ErrorMessage = "Fine amount is required.")]
        [Range(0.01, 10000.00, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}