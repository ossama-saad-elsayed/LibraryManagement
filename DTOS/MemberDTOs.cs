using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOS
{
    public class CreateMemberDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 200 characters.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string Address { get; set; } = null!;
    }

    public class UpdateMemberDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 200 characters.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string Address { get; set; } = null!;
    }


   
        public class MemberDto
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
            public string PhoneNumber { get; set; } = null!;
            public string Address { get; set; } = null!;
            public DateTime CreatedAt { get; set; }
        }
    



}
