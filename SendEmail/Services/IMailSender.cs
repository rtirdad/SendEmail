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
        public interface IMailSender
        {
            Task SendFakeEmailAsync(MailRequest mailRequest);
        }

    public class MailKitSender : IMailService
    {
        private readonly MailSettings _mailSettings;

        public async Task SendEmailAsync(MailRequest mailrequest)
        {
           
            /*using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailrequest.FromMail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);*/
        }
    }

    public class FakeMailSender : IMailSender
    {
        public Task SendFakeEmailAsync(MailRequest mailRequest)
        {
            throw new NotImplementedException();
        }
    }
}
    

