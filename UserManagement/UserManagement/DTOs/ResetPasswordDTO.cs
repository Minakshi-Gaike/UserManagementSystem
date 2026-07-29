namespace UserManagementProject.DTOs
{
    public class ResetPasswordDto
    {
        public string EmailAddress { get; set; }
        public string NewPaswword { get; set; }
        public string ConfirmNewPassword { get; set; }

    }
}