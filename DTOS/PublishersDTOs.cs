using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class PublisherDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class CreatePublisherDto
    {
        [Required(ErrorMessage = "Publisher name is required.")]
        [StringLength(150, ErrorMessage = "Publisher name is too long.")]
        public string Name { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? ContactNumber { get; set; }
    }
}