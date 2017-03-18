using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for AuthenticationFormSurfaceController
/// </summary>
public class AuthenticationFormSurfaceController : SurfaceController
{
    public ActionResult Index()
    {
        return PartialView("AuthenticationForm");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LogOff()
    {
        SessionManager.Clear();

        return CurrentUmbracoPage();
    }
}