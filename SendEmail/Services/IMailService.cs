using SendEmail.Models;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailrequest, List<IFormFile> attachments);

    }
}
