namespace UserManagement.DTOs
{
    public class AdminUserDTO
    {
  
            public int UserId { get; set; }

            public string UserName { get; set; }

            public string Gender { get; set; }

            public DateTime? BirthDate { get; set; }

            public string EmailAddress { get; set; }

            public string MobileNumber { get; set; }

            public string ProfilePhoto { get; set; }

            public string LocalAddress { get; set; }

            public DateTime? RegistrationDate { get; set; }

            public int IsActive { get; set; }
        }
    }

