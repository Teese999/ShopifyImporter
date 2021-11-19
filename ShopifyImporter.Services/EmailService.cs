using ShopifyImporter.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{
    public class EmailService : IEmailService
    {
        private Settings _settings;

        public EmailService(Settings settings)
        {
            _settings = settings;
        }

        public void Send(string from, string to, string subject, string html)
        {
            var email = new MailMessage();
            email.From = new MailAddress(from);
            var toAddresses = to.Split(',');
            foreach (var toAddress in toAddresses)
            {
                email.To.Add(toAddress);
            }
            email.Subject = subject;
            email.Body = html;

            using var smtp = new SmtpClient(_settings.Smtp.SmtpHost, _settings.Smtp.SmtpPort);
            smtp.Credentials = new System.Net.NetworkCredential(_settings.Smtp.SmtpUserName, _settings.Smtp.SmtpPassword);
            smtp.EnableSsl = true;
            smtp.Send(email);
        }
    }
}
