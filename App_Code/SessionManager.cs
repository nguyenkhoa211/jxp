using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SessionManager
/// </summary>
public class SessionManager
{
    public SessionManager() { }
    public static void Clear()
    {
        HttpContext.Current.Session.Clear();
    }
    public static void RemoveSession(string s)
    {
        HttpContext.Current.Session.Remove(s);
    }
    public static UserViewModel UserLogin
    {
        get
        {
            if (HttpContext.Current.Session["UserLogin"] == null)
            {
                return null;
            }
                
            return (UserViewModel)HttpContext.Current.Session["UserLogin"];
        }
        set
        {
            HttpContext.Current.Session["UserLogin"] = value;
        }
    }
}