namespace UserManagement.Models
{
    public class EmailSetting
    {
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int port { get; set; }
        public bool UseSSL { get; set; }
    }
}
