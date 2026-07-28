using System.ComponentModel.DataAnnotations;
namespace UserManagement.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage ="*")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "*")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "*")]

        public DateOnly? BirthDate { get; set; }

        [Required(ErrorMessage = "*")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "*")]
        public string? MobileNumber { get; set; }
        [Required(ErrorMessage = "*")]
        public int? RoleId { get; set; }
        //[Required(ErrorMessage = "*")]

        public int? RoleName { get; set; }

        //[Required(ErrorMessage = "*")]
        public string? ProfilePhoto { get; set; }

        public string? LocalAddress { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime? RegistrationDate { get; set; }

    }
}
