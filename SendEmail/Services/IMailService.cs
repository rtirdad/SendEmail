using SendEmail.Models;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailrequest);

    }

    public class FakeMailSender : IMailService
    {
        public Task SendEmailAsync(MailRequest mailrequest)
        {
            throw new NotImplementedException();
        }
    }

    public class MailKitSender : IMailService
    {
        public Task SendEmailAsync(MailRequest mailrequest)
        {
            throw new NotImplementedException();
        }
    }
}
