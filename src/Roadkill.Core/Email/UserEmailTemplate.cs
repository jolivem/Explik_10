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
    public abstract class UserEmailTemplate : EmailTemplate
    {
 
        /// <summary>
        /// Initializes a new instance of the <see cref="UserEmailTemplate"/> class.
        /// </summary>
        /// <param name="applicationSettings"></param>
        /// <param name="siteSettings"></param>
        /// <param name="emailClient">The <see cref="IEmailClient"/> to send the mail through. If this 
        /// parameter is null, then <see cref="EmailClient"/> is used</param>
        protected UserEmailTemplate(ApplicationSettings applicationSettings, SiteSettings siteSettings, IEmailClient emailClient)
            :base (applicationSettings, siteSettings, emailClient)
        {

        }

        /// <summary>
        /// Sends a notification email to the provided address, using the template provided.
        /// </summary>
        public virtual void Send(UserViewModel model, string subject)
        {
            if (model == null || (string.IsNullOrEmpty(model.ExistingEmail) && string.IsNullOrEmpty(model.NewEmail)))
                throw new EmailException(null, "The UserViewModel for the email is null or has an empty email");

            if (string.IsNullOrEmpty(PlainTextView))
                throw new EmailException(null, "No plain text view can be found for {0}", GetType().Name);

            if (string.IsNullOrEmpty(HtmlView))
                throw new EmailException(null, "No HTML view can be found for {0}", GetType().Name);

            string plainTextContent = ReplaceTokens(model, PlainTextView);
            string htmlContent = ReplaceTokens(model, HtmlView);

            string emailTo = model.ExistingEmail;
            if (string.IsNullOrEmpty(emailTo))
                emailTo = model.NewEmail;

            if (string.IsNullOrEmpty(emailTo))
                throw new EmailException(null, "The UserViewModel has an empty current or new email address");

            // Construct the message and the two views
            MailMessage message = new MailMessage();
            message.To.Add(emailTo);
            message.Subject = subject;
            Send(message, plainTextContent, htmlContent);
        }


        /// <summary>
        /// Replaces all tokens in the html and plain text views.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="template"></param>
        protected internal virtual string ReplaceTokens(UserViewModel model, string template)
        {
            string result = template;

            result = result.Replace("{FIRSTNAME}", model.Firstname);
            result = result.Replace("{LASTNAME}", model.Lastname);
            result = result.Replace("{EMAIL}", model.NewEmail);
            result = result.Replace("{USERNAME}", model.NewUsername);
            result = result.Replace("{SITEURL}", SiteSettings.SiteUrl);
            result = result.Replace("{ACTIVATIONKEY}", model.ActivationKey);
            result = result.Replace("{RESETKEY}", model.PasswordResetKey);
            result = result.Replace("{USERID}", model.Id.ToString());
            result = result.Replace("{SITENAME}", "Explik");

            if (HttpContext.Current != null)
                result = result.Replace("{REQUEST_IP}", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            return result;
        }
    }
}
