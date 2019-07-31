using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using Roadkill.Core.Configuration;
using System.Configuration;
using Roadkill.Core.Mvc.ViewModels;
using System.IO;
using System.Globalization;

namespace Roadkill.Core.Email
{
    /// <summary>
    /// A base class for an email template.
    /// </summary>
    /// <remarks>
    /// The following tokens are replaced inside the email templates:
    /// - {FIRSTNAME}
    /// - {LASTNAME}
    /// - {EMAIL}
    /// - {SITEURL}
    /// - {ACTIVATIONKEY}
    /// - {USERID}
    /// - {SITENAME}
    /// </remarks>
    public abstract class EmailTemplate
    {
        protected ApplicationSettings ApplicationSettings;
        protected SiteSettings SiteSettings;
        protected IEmailClient EmailClient;

        /// <summary>
        /// The HTML template for the email.
        /// </summary>
        public string HtmlView { get; set; }

        /// <summary>
        /// The plain text template for the email.
        /// </summary>
        public string PlainTextView { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplate"/> class.
        /// </summary>
        /// <param name="applicationSettings"></param>
        /// <param name="siteSettings"></param>
        /// <param name="emailClient">The <see cref="IEmailClient"/> to send the mail through. If this 
        /// parameter is null, then <see cref="EmailClient"/> is used</param>
        protected EmailTemplate(ApplicationSettings applicationSettings, SiteSettings siteSettings, IEmailClient emailClient)
        {
            ApplicationSettings = applicationSettings;
            SiteSettings = siteSettings;

            EmailClient = emailClient;
            if (EmailClient == null)
                EmailClient = new EmailClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="plainTextContent"></param>
        /// <param name="htmlContent"></param>
        public void Send(MailMessage message, string plainTextContent, string htmlContent)
        {

            AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(plainTextContent, new ContentType("text/plain"));
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlContent, new ContentType("text/html"));
            message.AlternateViews.Add(htmlView);
            message.AlternateViews.Add(plainTextView);

            // Add "~" support for pickupdirectories.
            if (EmailClient.GetDeliveryMethod() == SmtpDeliveryMethod.SpecifiedPickupDirectory &&
                !string.IsNullOrEmpty(EmailClient.PickupDirectoryLocation) &&
                EmailClient.PickupDirectoryLocation.StartsWith("~"))
            {
                string root = AppDomain.CurrentDomain.BaseDirectory;
                string pickupRoot = EmailClient.PickupDirectoryLocation.Replace("~/", root);
                pickupRoot = pickupRoot.Replace("/", @"\");
                EmailClient.PickupDirectoryLocation = pickupRoot;
            }

            // Do not send email if application is running in dev environment
            if (!ApplicationSettings.AttachmentsDirectoryPath.Contains("Explik_10_repo") &&
                !ApplicationSettings.AttachmentsDirectoryPath.Contains("TetrapolGtw"))
            {
                EmailClient.Send(message);
            }
        }

        /// <summary>
        /// Reads the text file prowvided from the email templates directory.
        /// If a culture-specific version of the file exists, e.g. /fr/signup.txt then this is used instead.
        /// </summary>
        protected internal string ReadTemplateFile(string filename)
        {
            string templatePath = ApplicationSettings.EmailTemplateFolder;
            string textfilePath = Path.Combine(templatePath, filename);
            string culturePath = Path.Combine(templatePath, CultureInfo.CurrentUICulture.Name);

            // If there's templates for the current culture, then use those instead
            if (Directory.Exists(culturePath))
            {
                string culturePlainTextFile = Path.Combine(culturePath, filename);
                if (File.Exists(culturePlainTextFile))
                    textfilePath = culturePlainTextFile;
            }

            return File.ReadAllText(textfilePath);
        }
    }
}
