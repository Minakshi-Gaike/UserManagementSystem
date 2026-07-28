using System.ComponentModel.DataAnnotations;
namespace UserManagement.DTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage ="*")]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage ="*")]
        public string? Password { get; set; }
    }
}
