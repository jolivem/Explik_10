﻿using Roadkill.Core.Configuration;
using Roadkill.Core.Logging;
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
        //private const string APIKey = "047a7b83a83d86371c92a3553dc2d782";
        //private const string SecretKey = "2d7faf66db712ba24d75dbc34fcb6a7e";
        private string from;

        public EmailClient(ApplicationSettings _applicationSettings)
		{
            from = _applicationSettings.SmtpFromEmail;

            _smtpClient = new SmtpClient(_applicationSettings.SmtpServerHost, _applicationSettings.SmtpServerPort);
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.EnableSsl = false;
            //_smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new System.Net.NetworkCredential(
                _applicationSettings.SmtpFromEmail, 
                _applicationSettings.SmtpFromPwd); // pwd generated by Ikoula
            //PickupDirectoryLocation = _smtpClient.PickupDirectoryLocation;
            //Log.Debug("SmtpServerHost: {0}", _applicationSettings.SmtpServerHost);
            //Log.Debug("SmtpServerPort: {0}", _applicationSettings.SmtpServerPort);
            //Log.Debug("SmtpFromEmail: {0}", _applicationSettings.SmtpFromEmail);
            //Log.Debug("SmtpFromPwd: {0}", _applicationSettings.SmtpFromPwd);

        }

        public void Send(MailMessage message)
        {
            Log.Debug("smtp subject : {0}", message.Subject);
            Log.Debug("smtp send from : {0}", message.From.ToString());
            Log.Debug("smtp send to : {0}", message.To.ToString());
            Log.Debug("smtp server address : {0}", _smtpClient.Host);
            Log.Debug("smtp server port : {0}", _smtpClient.Port);

            try
            {
                _smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Log.Error("STMP Client sending exception: {0}", ex);
                throw;
            }
        }

		public SmtpDeliveryMethod GetDeliveryMethod()
		{
			return _smtpClient.DeliveryMethod;
		}
	}
}
