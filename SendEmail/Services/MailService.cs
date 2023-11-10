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

namespace SendEmail.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IMailService _mailService;
        

        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
            //_mailService = mailService;
        }

        public async Task SendEmailAsync(MailRequest mailrequest)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(mailrequest.FromDisplayName, mailrequest.FromMail));
            //email.Sender = MailboxAddress.Parse(mailrequest.FromAppPassword);
            email.To.Add(new MailboxAddress(mailrequest.ToDisplayName, mailrequest.ToEmail));

            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();

            if (mailrequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailrequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();

            await _mailService.SendEmailAsync(mailrequest);


            /*using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailrequest.FromMail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);*/
        }
    }
}