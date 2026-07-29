using System.ComponentModel.DataAnnotations;

namespace UserManagementProject.DTOs
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string EmailAddress { get; set; }
        //public string NewPaswword { get; set; }
        //public string ConfirmNewPassword {  get; set; }

    }
}