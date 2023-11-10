using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using SendEmail.Models;
using SendEmail.Settings;
using System.Net.Mail;
using MimeKit.Text;

namespace SendEmail.Services
{
    public class FakeMail : IMailService
    {


        public void MailService() 
        {
            /*using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailrequest.FromMail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);*/
        }

        public Task SendEmailAsync(MailRequest mailrequest)
        {
            throw new NotImplementedException();
        }
    }
}
