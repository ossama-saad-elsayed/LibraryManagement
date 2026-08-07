using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class LoginDTO
    {
        public int Id { get; set; }
        [Required]
        public string Password { get; set; } = null!;
    }
}
