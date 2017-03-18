using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for LoginFormSurfaceController
/// </summary>
public class LoginFormSurfaceController : SurfaceController
{
    const int registerUsersOverviewNodeId = 1158;

    public ActionResult Index()
    {
        return PartialView("LoginForm", new LoginFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitForm(LoginFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var contentService = Services.ContentService;
        var user = contentService.GetChildrenByName(registerUsersOverviewNodeId, model.UserName).FirstOrDefault();
        
        if(user != null && user.Status == ContentStatus.Published && user.GetValue<string>("password") == CommonUtility.MD5Hash(model.Password))
        {
            SessionManager.UserLogin = new UserViewModel
            {
                UserName = user.GetValue<string>("userName"),
                FullName = user.GetValue<string>("fullName"),
                Email = user.GetValue<string>("email"),
                CompanyName = user.GetValue<string>("company"),
                CompanyAddress = user.GetValue<string>("address")
            };
            return RedirectToUmbracoPage(1214);
        }
        else
        {
            ModelState.AddModelError("", "Invalid username or password or user is not published.");
            //TempData["LoginFail"] = true;
        }

        return CurrentUmbracoPage();
    }
}