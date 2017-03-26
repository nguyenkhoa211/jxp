using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

/// <summary>
/// Summary description for SignUpSurfaceController
/// </summary>
public class SignUpFormSurfaceController : SurfaceController
{
    const int registerUsersOverviewNodeId = 1158;
    const int companyOverviewNodeId = 1322;
    private string senderEmail = WebConfigurationManager.AppSettings["SenderEmail"].ToString();
    private string senderPassword = WebConfigurationManager.AppSettings["SenderPassword"].ToString();
    private string administratorEmail = WebConfigurationManager.AppSettings["AdministratorEmail"].ToString();

    public ActionResult Index()
    {
        return PartialView("SignUpForm", new SignUpFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitForm(SignUpFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }

        var contentService = Services.ContentService;

        var user = contentService.GetChildrenByName(registerUsersOverviewNodeId, model.UserName).FirstOrDefault();
        if (user == null || String.IsNullOrEmpty(user.GetValue<string>("userName")))
        {
            var isAdd = true;
            
            var emails = contentService.GetChildren(registerUsersOverviewNodeId);
            foreach(var e in emails)
            {
                if(e != null && e.GetValue<string>("email") != null && String.Equals(e.GetValue<string>("email"), model.Email, StringComparison.OrdinalIgnoreCase))
                {
                    isAdd = false;
                    break;
                }
            }
            
            if(isAdd)
            {
                var registerUser = contentService.CreateContent(model.UserName, registerUsersOverviewNodeId, "registerUserItem", 0);
                registerUser.SetValue("userName", model.UserName);
                registerUser.SetValue("password", CommonUtility.MD5Hash(model.Password));
                registerUser.SetValue("fullName", model.FullName);
                registerUser.SetValue("email", model.Email);
                registerUser.SetValue("company", model.Company);
                registerUser.SetValue("address", model.Address);
                //contentService.SaveAndPublishWithStatus(registerUser);
                contentService.Save(registerUser);

                var companies = contentService.GetChildrenByName(companyOverviewNodeId, model.Company);
                if (companies.Count() <= 0)
                {
                    var companyItem = contentService.CreateContent(model.Company, companyOverviewNodeId, "companyItem", 0);
                    companyItem.SetValue("address", model.Address);
                    contentService.SaveAndPublishWithStatus(companyItem);
                }

                /*
                // Send an email to admin
                string subject = "New customer has just registerd an account";
                string body = string.Format("New customer {0} has just registered. Please publish them on Sign Up > Register Users node tree at back office. Sent by JXP Website.", model.UserName);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                using (var message = new MailMessage(senderEmail, administratorEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                {
                    await smtp.SendMailAsync(message);
                }
                */

                /*
                string body = string.Format("New customer {0} has just registered. Please publish them on Sign Up > Register Users node tree at back office. Sent by JXP Website.", model.UserName);
                string subject = "New customer has just registerd an account";

                //Create the msg object to be sent
                MailMessage msg = new MailMessage();
                //Add your email address to the recipients
                msg.To.Add(administratorEmail);
                //Configure the address we are sending the mail from
                MailAddress address = new MailAddress("no-reply@hitecagency.com");
                msg.From = address;
                msg.Subject = subject;
                msg.Body = body;
                msg.IsBodyHtml = true;

                //Configure an SmtpClient to send the mail.
                using (SmtpClient client = new SmtpClient())
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = false;
                    client.Host = "relay-hosting.secureserver.net";
                    client.Port = 25;

                    //Setup credentials to login to our sender email address ("UserName", "Password")
                    NetworkCredential credentials = new NetworkCredential("no-reply@hitecagency.com", "");
                    client.UseDefaultCredentials = true;
                    client.Credentials = credentials;

                    await client.SendMailAsync(msg);
                }
                */

                TempData["FormSubmitted"] = true;
            }
            else
            {
                TempData["UserExisted"] = true;
            }
        }
        else
        {
            TempData["UserExisted"] = true;
        }

        return CurrentUmbracoPage();
    }
}