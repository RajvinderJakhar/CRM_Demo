using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CRM_Core.Utility
{
    public sealed class EmailProvider : SendGridEmailProvider, IEmailProvider
    {
        #region Fields
        private readonly AppSettings _appSettings;
        #endregion
        #region Contructors
        public EmailProvider(IOptions<AppSettings> appSettings)
            : base(appSettings)
        {
            _appSettings = appSettings.Value;
        }
        #endregion

        #region Send Mail
        public bool Send(
            string to,
            string subject,
            string body,
            string cc = null,
            string bcc = null,
            string from = null,
            string fromDisplayName = null,
            MailPriority mailPriority = MailPriority.Normal)
        {
            return Send(to, subject, body, cc, bcc, from, fromDisplayName, null, mailPriority);
        }

        public async Task SendAsync(
            string to,
            string subject,
            string body,
            string cc = null,
            string bcc = null,
            string from = null,
            string fromDisplayName = null,
            MailPriority mailPriority = MailPriority.Normal)
        {
            SendAsync(to, subject, body, cc, bcc, from, fromDisplayName, null, mailPriority);
        }

        private bool Send(
            string to,
            string subject,
            string body,
            string cc,
            string bcc,
            string from,
            string fromDisplayName,
            string plainText,
            MailPriority mailPriority)
        {
            if (String.IsNullOrEmpty(_appSettings.No_Reply_Email) || String.IsNullOrEmpty(_appSettings.No_Reply_Password))
                throw new ArgumentNullException("Sender email and password is required!");

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            if (!String.IsNullOrEmpty(cc))
                message.CC.Add(cc);
            if (!String.IsNullOrEmpty(bcc))
                message.Bcc.Add(bcc);
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(fromDisplayName))
            {
                message.From = new MailAddress(_appSettings.No_Reply_Email, fromDisplayName);
                message.Sender = new MailAddress(from, fromDisplayName);
                message.ReplyToList.Add(new MailAddress(from, fromDisplayName));
            }
            else
            {
                message.From = new MailAddress(_appSettings.No_Reply_Email);
            }
            message.Subject = subject;

            message.Priority = mailPriority;


            if (!String.IsNullOrEmpty(plainText))
            {
                message.IsBodyHtml = true;
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainText, new ContentType("text/plain")));
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html")));
            }
            else
            {
                message.Body = body;
                message.IsBodyHtml = true;
            }

            //message.Headers.Add("message-id", "<" + DateTime.Now.Ticks + "@FillContracts.com>");
            message.Headers.Add("Message-Id", String.Concat("<", DateTime.Now.ToString("yyMMdd"), "-", DateTime.Now.ToString("HHmmss"), "-", DateTime.Now.Ticks.ToString(), "@fillcontracts.com>"));

            message.BodyEncoding = Encoding.GetEncoding("utf-8");

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = _appSettings.No_Reply_Email,
                        Password = _appSettings.No_Reply_Password
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = _appSettings.SMTP_Host;    // "smtp.gmail.com";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = Convert.ToInt32(_appSettings.SMTP_Port);    //587;
                    ////For set enable ssl true need port 465 or 587 and ssl certificate installed on mail server
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                    return true;
                }
            }
            catch (SmtpException Ex)
            {
                throw new Exception(Ex.Message, Ex.InnerException ?? Ex);
                //return false;
            }

        }

        private async Task SendAsync(
            string to,
            string subject,
            string body,
            string cc,
            string bcc,
            string from,
            string fromDisplayName,
            string plainText,
            MailPriority mailPriority)
        {
            if (String.IsNullOrEmpty(_appSettings.No_Reply_Email) || String.IsNullOrEmpty(_appSettings.No_Reply_Password))
                throw new ArgumentNullException("Sender email and password is required!");

            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            if (!String.IsNullOrEmpty(cc))
                message.CC.Add(cc);
            if (!String.IsNullOrEmpty(bcc))
                message.Bcc.Add(bcc);
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(fromDisplayName))
            {
                message.From = new MailAddress(_appSettings.No_Reply_Email, fromDisplayName);
                message.Sender = new MailAddress(from, fromDisplayName);
                message.ReplyToList.Add(new MailAddress(from, fromDisplayName));
            }
            else
            {
                message.From = new MailAddress(_appSettings.No_Reply_Email);
            }
            message.Subject = subject;

            message.Priority = mailPriority;


            if (!String.IsNullOrEmpty(plainText))
            {
                message.IsBodyHtml = true;
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(plainText, new ContentType("text/plain")));
                message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html")));
            }
            else
            {
                message.Body = body;
                message.IsBodyHtml = true;
            }

            //message.Headers.Add("message-id", "<" + DateTime.Now.Ticks + "@FillContracts.com>");
            message.Headers.Add("Message-Id", String.Concat("<", DateTime.Now.ToString("yyMMdd"), "-", DateTime.Now.ToString("HHmmss"), "-", DateTime.Now.Ticks.ToString(), "@fillcontracts.com>"));

            message.BodyEncoding = Encoding.GetEncoding("utf-8");

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = _appSettings.No_Reply_Email,
                        Password = _appSettings.No_Reply_Password
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = _appSettings.SMTP_Host;    // "smtp.gmail.com";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = Convert.ToInt32(_appSettings.SMTP_Port);    //587;
                    ////For set enable ssl true need port 465 or 587 and ssl certificate installed on mail server
                    smtp.EnableSsl = true;
                    smtp.SendMailAsync(message);
                }
            }
            catch (SmtpException Ex)
            {
                throw new Exception(Ex.Message, Ex.InnerException ?? Ex);
            }

        }
        #endregion
    }
}
