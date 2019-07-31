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
    public abstract class PageEmailTemplate : EmailTemplate
    {
 
        /// <summary>
        /// Initializes a new instance of the <see cref="PageEmailTemplate"/> class.
        /// </summary>
        /// <param name="applicationSettings"></param>
        /// <param name="siteSettings"></param>
        /// <param name="emailClient">The <see cref="IEmailClient"/> to send the mail through. If this 
        /// parameter is null, then <see cref="EmailClient"/> is used</param>
        protected PageEmailTemplate(ApplicationSettings applicationSettings, SiteSettings siteSettings, IEmailClient emailClient)
            :base (applicationSettings, siteSettings, emailClient)
        {
            //ApplicationSettings = applicationSettings;
            //SiteSettings = siteSettings;

            //EmailClient = emailClient;
            //if (EmailClient == null)
            //    EmailClient = new EmailClient();
        }

        /// <summary>
        /// Sends a notification email to the provided address, using the template provided.
        /// </summary>
        public virtual void Send(PageEmailInfo info, string subject)
        {
#if !DEBUG
            //TODO TODO TODO
            if (info == null || string.IsNullOrEmpty(info.User.Email))
                throw new EmailException(null, "The UserViewModel for the email is null or has an empty email");

            if (string.IsNullOrEmpty(PlainTextView))
                throw new EmailException(null, "No plain text view can be found for {0}", GetType().Name);

            if (string.IsNullOrEmpty(HtmlView))
                throw new EmailException(null, "No HTML view can be found for {0}", GetType().Name);

            string plainTextContent = ReplaceTokens(info, PlainTextView);
            string htmlContent = ReplaceTokens(info, HtmlView);

            string emailTo = info.User.Email;

            if (string.IsNullOrEmpty(emailTo))
                throw new EmailException(null, "The PageEmailInfo has an empty email address");

            // Construct the message and the two views
            MailMessage message = new MailMessage();
            message.To.Add(emailTo);
            message.Subject = subject;
            Send(message, plainTextContent, htmlContent);
#endif
        }

        /// <summary>
        /// Replaces all tokens in the html and plain text views.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="template"></param>
        protected internal virtual string ReplaceTokens(PageEmailInfo info, string template)
        {
            string result = template;

            result = result.Replace("{FIRSTNAME}", info.User.Firstname);
            result = result.Replace("{LASTNAME}", info.User.Lastname);
            result = result.Replace("{PAGETITLE}", info.Page.Title);
            result = result.Replace("{REJECTREASON}", info.RejectType);
            ////result = result.Replace("{ACTIVATIONKEY}", model.ActivationKey);
            //result = result.Replace("{RESETKEY}", model.PasswordResetKey);
            //result = result.Replace("{USERID}", model.Id.ToString());
            //result = result.Replace("{SITENAME}", SiteSettings.SiteName);

            return result;
        }
    }
}
