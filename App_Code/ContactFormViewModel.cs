using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ContactFormViewModel
/// </summary>
public class ContactFormViewModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Subject { get; set; }

    public string Message { get; set; }
}