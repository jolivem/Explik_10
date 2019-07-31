using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Roadkill.Core;
using Roadkill.Core.Configuration;
using Roadkill.Core.Email;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Tests.Unit
{
	public class EmailTemplateStub : EmailTemplate
	{
		public EmailTemplateStub(ApplicationSettings applicationSettings, SiteSettings siteSettings, IEmailClient emailClient)
			: base(applicationSettings, siteSettings, emailClient)
		{
			base.PlainTextView = "plaintextview";
			base.HtmlView = "htmlview";
		}
		
  //      public override void Send(MailMessage message, string plainTextContent, string htmlContent)
  //      {
		//	base.Send(message, plainTextContent, htmlContent);
		//}

		public IEmailClient GetEmailClient()
		{
			return base.EmailClient;
		}
	}
}
