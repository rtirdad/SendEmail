using MailKit.Security;
using SendEmail.Models;
using SendEmail.Settings;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailrequest);

    }

    public class FakeMailSender : IMailService
    {
        public MailRequest LastMailRequest { get; set; }

        //public string ToEmail { get; set; }

        //public string ToDisplayName { get; set; }

        //public string FromDisplayName { get; set; }

        //public string FromMail { get; set; }

        //public string Subject { get; set; }

        //ublic string Body { get; set; }

        //public IFormFileCollection? Attachments { get; set; }

        public Task SendEmailAsync(MailRequest mailrequest)
        {
            //ToEmail = mailrequest.ToEmail;

            //ToDisplayName = mailrequest.ToDisplayName;

            //FromDisplayName = mailrequest.FromDisplayName;

            //FromMail = mailrequest.FromMail;

            //Subject = mailrequest.Subject;

            //Body = mailrequest.Body;

            //Attachments = mailrequest.Attachments;

            LastMailRequest = mailrequest;
            return Task.CompletedTask;
        }
    }

    public class MailKitSender : IMailService
    {
        
        private readonly MailSettings _mailSettings;

        public MailKitSender(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }
        public Task SendEmailAsync(MailRequest mailrequest)
        {
            throw new NotImplementedException();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailrequest.FromMail, _mailSettings.Password);
            //await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
