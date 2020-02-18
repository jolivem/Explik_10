using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Globalization;
using Roadkill.Core.Configuration;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Localization;
using Roadkill.Core.Logging;
using System.Net.Mail;

namespace Roadkill.Core.Email
{
    /// <summary>
    /// The template for signup emails.
    /// </summary>
    public class SignupEmail : UserEmailTemplate
    {
        private static string _htmlContent;
        private static string _plainTextContent;
 
        public SignupEmail(ApplicationSettings applicationSettings, SiteSettings siteSettings, IEmailClient emailClient)
            : base(applicationSettings, siteSettings, emailClient)
        {
        }

        public void Send(UserViewModel model)
        {
            // Thread safety should not be an issue here
            if (string.IsNullOrEmpty(_plainTextContent))
                _plainTextContent = ReadTemplateFile("Signup.txt");

            if (string.IsNullOrEmpty(_htmlContent))
                _htmlContent = ReadTemplateFile("Signup.html");

            PlainTextView = _plainTextContent;
            HtmlView = _htmlContent;

            base.Send(model, SiteStrings.EmailSubject_AccountActivation);

            SendToVips(model);

        }

        public void SendToVips(UserViewModel model)
        {
            try
            {
                string htmlContent = "<html>" +
                    "<body style='font-family:Arial;font-size:1em'>" +
                    "<p>Inscription de " + model.Firstname + " " + model.Lastname + " !</p>" +
                    "</body></html>";
                string plainTextContent = "Inscription de " + model.Firstname + " " + model.Lastname + ".";
                MailMessage message = new MailMessage(
                        new MailAddress(FromEmail, "Explik"), new MailAddress("jolivet.michel@free.fr"));
                message.Subject = "Nouvelle inscription";
                base.SendToVip(message , htmlContent, plainTextContent);
            }
            catch (Exception ex)
            {
                Log.Error("STMP Client sending to VIP exception: {0}", ex);
                // do not forward throw
            }
        }
    }
}

