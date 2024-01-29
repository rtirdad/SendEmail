using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using SendEmail.Models;
using SendEmail.Settings;
using System.Net.Mail;
using MimeKit.Text;

using MailKit.Security;
using System.Text.Json;

namespace SendEmail.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }

        public async Task SendEmailAsync(MailRequest mailrequest)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(mailrequest.FromDisplayName, mailrequest.FromMail));
            //email.Sender = MailboxAddress.Parse(mailrequest.FromAppPassword);
            email.To.Add(new MailboxAddress(mailrequest.ToDisplayName, mailrequest.ToEmail));

            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();

            byte[] fileBytes;
            if (System.IO.File.Exists("Attachment/report.pdf"))
            {
                FileStream file = new FileStream("Attachment/report.pdf", FileMode.Open, FileAccess.Read);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                builder.Attachments.Add("attachment.pdf", fileBytes, ContentType.Parse("application/pdf"));
                //builder.Attachments.Add("attachment2.pdf", fileBytes, ContentType.Parse("application/octet-stream"));
            }

            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailrequest.FromMail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public void GeneratePDF(JsonDocument doc, HttpContext context) 
        {
        
        }
    }
}