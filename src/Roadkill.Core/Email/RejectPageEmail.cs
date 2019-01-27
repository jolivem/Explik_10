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
using Roadkill.Core.Mvc.Controllers;

namespace Roadkill.Core.Email
{
	/// <summary>
	/// The template for password reset emails.
	/// </summary>
	public class RejectPageEmail : PageEmailTemplate
	{
		private static string _htmlContent;
		private static string _plainTextContent;

		public RejectPageEmail(ApplicationSettings applicationSettings, SiteSettings siteSettings, IEmailClient emailClient)
			: base(applicationSettings, siteSettings, emailClient)
		{
		}

		public void Send(PageEmailInfo info)
		{
            if (info.RejectType == null)
            {
                return;
            }

            string rejectReason = "";
            switch (info.RejectType)
            {
                case PagesController.AlertLanguage:
                    rejectReason = SiteStrings.Alert_Reason_Language;
                    break;
                case PagesController.AlertPublicity:
                    rejectReason = SiteStrings.Alert_Reason_Publicity;
                    break;
                case PagesController.AlertRespect:
                    rejectReason = SiteStrings.Alert_Reason_Disrespectful;
                    break;
                case PagesController.AlertControversial:
                    rejectReason = SiteStrings.Alert_Reason_Controversial;
                    break;
                case PagesController.AlertOther:
                    rejectReason = SiteStrings.Alert_Reason_Other;
                    break;

            }

            // Thread safety should not be an issue here
            if (string.IsNullOrEmpty(_plainTextContent))
				_plainTextContent = ReadTemplateFile("RejectPage.txt");

			if (string.IsNullOrEmpty(_htmlContent))
				_htmlContent = ReadTemplateFile("RejectPage.html");

			PlainTextView = _plainTextContent;
			HtmlView = _htmlContent;
            string subject = string.Format("Votre page \""+ info.Page.Title + "\"");

			base.Send(info, subject);
		}
	}
}
