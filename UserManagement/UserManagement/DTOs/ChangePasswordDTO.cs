namespace UserManagement.DTOs
{
    public class ChangePasswordDTO
    {
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }


    }
}
