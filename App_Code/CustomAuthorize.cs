using System;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Summary description for CustomAuthorize
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class CustomAuthorize : AuthorizeAttribute
{
    public override void OnAuthorization(AuthorizationContext filterContext)
    {

        try
        {
            if (SessionManager.UserLogin == null)
            {
                filterContext.Result = new RedirectResult("/login");
            }
        }
        catch (Exception ex)
        {
            throw new HttpException(403, "Unauthorization");
        }
    }
}