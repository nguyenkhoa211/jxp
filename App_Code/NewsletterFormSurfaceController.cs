using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for NewsletterFormSurfaceController
/// </summary>
public class NewsletterFormSurfaceController : SurfaceController
{
    const int newsletterOverviewNodeId = 1299;

    public ActionResult Index()
    {
        return PartialView("NewsletterForm", new NewsletterViewModel());
    }

    public JsonResult SubmitForm(string email)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { Success = false, Message = "Problem!" });
        }

        var contentService = Services.ContentService;
        var newsletter = contentService.CreateContent("Guess, Id: " + Guid.NewGuid(), newsletterOverviewNodeId, "newsletterItem", 0);
        newsletter.SetValue("email", email);
        contentService.SaveAndPublishWithStatus(newsletter);

        //TempData["FormSubmitted"] = true;

        return Json(new { Success = true, Message = "Thank you!" });
    }
}