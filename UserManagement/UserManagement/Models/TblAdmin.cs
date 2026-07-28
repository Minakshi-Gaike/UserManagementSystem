using System;
using System.Collections.Generic;

namespace UserManagement.Models;

public partial class TblAdmin
{
    public int AdminId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }
}
