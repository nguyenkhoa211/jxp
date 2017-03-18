using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for ContactUsFormSurfaceController
/// </summary>
public class ContactUsFormSurfaceController : SurfaceController
{
    const int contactOverviewNodeId = 1271;

    public ActionResult Index()
    {
        return PartialView("ContactUsForm", new ContactFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitForm(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var contentService = Services.ContentService;
        var contact = contentService.CreateContent(model.Name + ", Id: " + Guid.NewGuid(), contactOverviewNodeId, "contactItem", 0);
        contact.SetValue("fullName", model.Name);
        contact.SetValue("email", model.Email);
        contact.SetValue("subject", model.Subject);
        contact.SetValue("message", model.Message);
        contentService.SaveAndPublishWithStatus(contact);

        TempData["FormSubmitted"] = true;

        return CurrentUmbracoPage();
    }
}