using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Utility
{
    public class SendGridEmailProvider
    {
        #region Fields
        private readonly AppSettings _appSettings;
        #endregion
        #region Contructors
        public SendGridEmailProvider(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        #endregion

        #region Send Mail
        public async Task<Response> SendEmailUsingSendGrid(
            string to,
            string subject,
            string body)
        {
            return await SendEmailAsync(to, subject, body);
        }
        public Task<Response> SendEmailAsync(
            string to,
            string subject,
            string body)
        {
            var client = new SendGridClient(_appSettings.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_appSettings.SendGridEmail, _appSettings.SendGridName ?? _appSettings.SendGridEmail),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body
            };
            msg.AddTo(new EmailAddress(to));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
        #endregion
    }
}
