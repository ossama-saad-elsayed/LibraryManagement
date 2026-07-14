using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public int PublicationYear { get; set; }
        public int CopiesOwned { get; set; }
        public int AvailableCopies { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }

        // Flattened properties for easy display
        public string AuthorName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string PublisherName { get; set; } = null!;
    }


    public class CreateBookDto
    {
        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "ISBN is required.")]
        public string ISBN { get; set; } = null!;

        [Required(ErrorMessage = "Publication year is required.")]
        [Range(1000, 2026, ErrorMessage = "Please enter a valid publication year.")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Copies owned count is required.")]
        [Range(1, 1000, ErrorMessage = "Copies owned must be at least 1.")]
        public int CopiesOwned { get; set; }

        [Required(ErrorMessage = "Author identity is required.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Category identity is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Publisher identity is required.")]
        public int PublisherId { get; set; }
    }
}