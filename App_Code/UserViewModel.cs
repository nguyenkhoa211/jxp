using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserViewModel
/// </summary>
public class UserViewModel
{
    public UserViewModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
}