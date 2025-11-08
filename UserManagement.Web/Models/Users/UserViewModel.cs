using System;
using System.ComponentModel.DataAnnotations;
using UserManagement.Web.Models.Logs;
using UserManagement.Web.Validation;

namespace UserManagement.Web.Models.Users;

public class UserViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    [StringLength(75, ErrorMessage = "Forename cannot exceed 75 characters")]
    public string Forename { get; set; } = string.Empty;

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(75, ErrorMessage = "Surname cannot exceed 75 characters")]
    public string Surname { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address entered")]
    [StringLength(75, ErrorMessage = "Email cannot exceed 75 characters")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Account Status")]
    public bool IsActive { get; set; }

    [Display(Name = "Date Of Birth")]
    [DataType(DataType.Date)]
    [DateOfBirthValidation]
    public DateTime? DateOfBirth { get; set; }

    // Adding logs to View page
    public List<UserActivityLogViewModel> UserActivityLogs { get; set; } = new();
}
