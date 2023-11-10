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
        public MailRequest LastMailRequest { get; private set; }

        public Task SendEmailAsync(MailRequest mailrequest)
        {
            // Save the last mail request for testing purposes
            LastMailRequest = mailrequest;

            // You can implement additional logic here if needed
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
            /*using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailrequest.FromMail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);*/
        }
    }
}
