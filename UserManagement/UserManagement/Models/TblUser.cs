using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? EmailAddress { get; set; }

    public string? MobileNumber { get; set; }

    public int? RoleId { get; set; }

    public string? ProfilePhoto { get; set; }

    public string? LocalAddress { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public int? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? Flag { get; set; }

    public string? Password { get; set; }

    public virtual TblRole? Role { get; set; }
}
