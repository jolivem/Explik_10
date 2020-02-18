using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Globalization;
using Roadkill.Core.Configuration;
using Roadkill.Core.Mvc.ViewModels;
using Roadkill.Core.Logging;
using System.Net.Mail;

namespace Roadkill.Core.Email
{
	/// <summary>
	/// The template for password reset emails.
	/// </summary>
	public class PublishPageEmail : PageEmailTemplate
	{
		private static string _htmlContent;
		private static string _plainTextContent;

		public PublishPageEmail(ApplicationSettings applicationSettings, SiteSettings siteSettings, IEmailClient emailClient)
			: base(applicationSettings, siteSettings, emailClient)
		{
		}

		public void Send(PageEmailInfo info)
		{
			// Thread safety should not be an issue here
			if (string.IsNullOrEmpty(_plainTextContent))
				_plainTextContent = ReadTemplateFile("PublishPage.txt");

			if (string.IsNullOrEmpty(_htmlContent))
				_htmlContent = ReadTemplateFile("PublishPage.html");

			PlainTextView = _plainTextContent;
			HtmlView = _htmlContent;
            string subject = string.Format("Votre page \"" + info.Page.Title + "\"");
            base.Send(info, subject);

            SendToVips(info);

        }

        public void SendToVips(PageEmailInfo info)
        {
            try
            {
                string htmlContent = "<html>" +
                        "<body style='font-family:Arial;font-size:1em'>" +
                        "<p>Titre : " + info.Page.Title + "</p>" +
                        "<p>Auteur : " + info.User.Firstname + " " + info.User.Lastname + "</p>" +
                        "</body></html>";
                string plainTextContent =
                    "Titre : " + info.Page.Title + "." +
                    "Auteur : " + info.User.Firstname + " " + info.User.Lastname + ".";

                MailMessage message = new MailMessage(
                        new MailAddress(FromEmail, "Explik"), new MailAddress("jolivet.michel@free.fr"));
                message.Subject = "Nouvelle page à controler";

                base.SendToVip(message, htmlContent, plainTextContent);
            }
            catch (Exception ex)
            {
                Log.Error("STMP Client sending to VIP exception: {0}", ex);
                // do not forward throw
            }
        }
    }
}
