using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Models;

namespace UserManagement.Web.Models.Logs;

public class Log
{
    [Key, DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public long Id { get; set;}
    public long? UserId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    [Required]
    [MaxLength(500)]
    public string Details { get; set; } = string.Empty;
    [Required]
    public DateTime TimeStamp { get; set; }

    // What else could we need?
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
