using SendGrid;
using System.Net.Mail;

namespace CRM_Core.Utility
{
    public interface IEmailProvider
    {
        bool Send(
            string to,
            string subject,
            string body,
            string cc = null,
            string bcc = null,
            string from = null,
            string fromDisplayName = null,
            MailPriority mailPriority = MailPriority.Normal);

        Task SendAsync(
            string to,
            string subject,
            string body,
            string cc = null,
            string bcc = null,
            string from = null,
            string fromDisplayName = null,
            MailPriority mailPriority = MailPriority.Normal);

        Task<Response> SendEmailUsingSendGrid(
            string to,
            string subject,
            string body);
    }
}
