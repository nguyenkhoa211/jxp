using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Umbraco.Web.WebApi;

/// <summary>
/// Summary description for NewsletterApiController
/// </summary>
public class NewsletterApiController : UmbracoApiController
{
    const int newsletterOverviewNodeId = 1299;

    public HttpResponseMessage SubmitForm(string email)
    {
        var contentService = Services.ContentService;
        var newsletter = contentService.CreateContent("Guess, Id: " + Guid.NewGuid(), newsletterOverviewNodeId, "newsletterItem", 0);
        newsletter.SetValue("email", email);
        contentService.SaveAndPublishWithStatus(newsletter);

        return Request.CreateResponse(HttpStatusCode.OK, new { Success = true, Message = "Thank you!" });
    }
}