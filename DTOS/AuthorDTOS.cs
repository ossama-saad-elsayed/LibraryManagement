using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Biography { get; set; }

    }

    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Biography cannot exceed 500 characters.")]
        public string? Biography { get; set; }
    }

}
