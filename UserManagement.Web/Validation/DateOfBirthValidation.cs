using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Validation;

public class DateOfBirthValidation : ValidationAttribute
{
    public DateOfBirthValidation()
    {
        ErrorMessage = "Invalid Date of Birth, the date of birth cannot be in the future.";
    }

    public override bool IsValid(object? value)
    {
        return value switch
        {
            null => false,
            DateTime date => date <= DateTime.Today,
            _ => false
        };
    }

}
