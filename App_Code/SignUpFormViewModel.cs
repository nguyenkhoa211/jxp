using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SignUpViewModel
/// </summary>
public class SignUpFormViewModel
{
    [Required]
    [StringLength(16, ErrorMessage = "Must be between 3 and 16 characters", MinimumLength = 3)]
    [Display(Name = "User name (*)")]
    public string UserName { get; set; }

    [Required]
    [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Display(Name = "Password (*)")]
    public string Password { get; set; }

    [Required]
    [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [Display(Name = "Confirm password (*)")]
    public string ConfirmPassword { get; set; }

    [Required]
    [Display(Name = "Full name (*)")]
    public string FullName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    [Display(Name = "Email address (*)")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Company name (*)")]
    public string Company { get; set; }

    [Display(Name = "Company address")]
    public string Address { get; set; }
}