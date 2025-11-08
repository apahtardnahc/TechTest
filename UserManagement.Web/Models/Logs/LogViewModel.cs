using System;

namespace UserManagement.Web.Models.Logs;

// For Viewing users individually 
public class LogViewModel
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public string? UserForename { get; set; } = string.Empty;
    public string? UserSurname { get; set; } = string.Empty;
}
