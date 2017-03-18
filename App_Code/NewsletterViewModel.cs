using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NewsletterViewModel
/// </summary>
public class NewsletterViewModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    [Display(Name = "Email address")]
    public string Email { get; set; }
}