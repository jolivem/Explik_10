using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Email
{
	public class EmailClient : IEmailClient
	{
		private SmtpClient _smtpClient;
		public string PickupDirectoryLocation { get; set; }
        
        // see https://app.mailjet.com/transactional for keys
        private const string APIKey = "047a7b83a83d86371c92a3553dc2d782";
        private const string SecretKey = "2d7faf66db712ba24d75dbc34fcb6a7e";

        private const string From = "contact@explik.net";
        //private const string To = "recipient@example.com";
        public EmailClient()
		{

            _smtpClient = new SmtpClient("in.mailjet.com", 587);
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.EnableSsl = true;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new System.Net.NetworkCredential(APIKey, SecretKey);
            // Default it to the SmtpClient's settings, which are read from a .config
            //PickupDirectoryLocation = _smtpClient.PickupDirectoryLocation;
        }

		public void Send(MailMessage message)
		{
			_smtpClient.PickupDirectoryLocation = PickupDirectoryLocation;
			_smtpClient.Send(message);
		}

		public SmtpDeliveryMethod GetDeliveryMethod()
		{
			return _smtpClient.DeliveryMethod;
		}
	}
}
