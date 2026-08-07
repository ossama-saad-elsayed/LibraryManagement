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
        [Range(1000, 2100, ErrorMessage = "Please enter a valid publication year.")]
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

    public class CreateBookByNamesDto
    {
        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        public string ISBN { get; set; } = null!;

        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(250, ErrorMessage = "Book title cannot exceed 250 characters.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Publication year is required.")]
        [Range(1000, 2100, ErrorMessage = "Publication year must be a valid year between 1000 and 2100.")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Copies owned is required.")]
        [Range(1, 10000, ErrorMessage = "Copies owned must be between 1 and 10,000.")]
        public int CopiesOwned { get; set; }

        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(150, ErrorMessage = "Author name cannot exceed 150 characters.")]
        public string AuthorName { get; set; } = null!;

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string CategoryName { get; set; } = null!;

        [Required(ErrorMessage = "Publisher name is required.")]
        [StringLength(150, ErrorMessage = "Publisher name cannot exceed 150 characters.")]
        public string PublisherName { get; set; } = null!;
    }

    public class UpdateBookByNamesDto : CreateBookByNamesDto
    {
        [Required(ErrorMessage = "Book ID is required for update.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Book ID.")]
        public int Id { get; set; }
    }
}