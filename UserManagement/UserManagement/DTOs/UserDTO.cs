namespace UserManagement.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }

        public string? Gender { get; set; }

        public DateOnly? BirthDate { get; set; }

        public string? EmailAddress { get; set; }


        public string? MobileNumber { get; set; }

        public int? RoleName { get; set; }

        public string? ProfilePhoto { get; set; }

        public string? LocalAddress { get; set; }

    }
}
