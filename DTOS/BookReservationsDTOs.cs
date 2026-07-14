using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class BookReservationDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; } = null!;

        public string BookTitle { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }

    public class CreateBookReservationDto
    {
        [Required(ErrorMessage = "BookId is required.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
    }
}