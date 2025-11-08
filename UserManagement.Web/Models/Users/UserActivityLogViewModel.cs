using System;

namespace UserManagement.Web.Models.Users;

public class UserActivityLogViewModel
{
    public long Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
}
