using SendEmail.Models;
using System.Threading.Tasks;

namespace SendEmail.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailrequest);

    }

    public class DecoyMail : IMailService
    {
        public Task SendEmailAsync(MailRequest mailrequest)
        {
            Console.WriteLine("DecoyMail: Email sending is disabled for testing or placeholder purposes.");

            // Access properties of the MailRequest model directly
            Console.WriteLine($"ToEmail: {mailrequest.ToEmail}");
            Console.WriteLine($"ToDisplayName: {mailrequest.ToDisplayName}");
            Console.WriteLine($"FromDisplayName: {mailrequest.FromDisplayName}");
            Console.WriteLine($"FromMail: {mailrequest.FromMail}");
            Console.WriteLine($"Subject: {mailrequest.Subject}");
            Console.WriteLine($"Body: {mailrequest.Body}");

            if (mailrequest.Attachments != null)
            {
                foreach (var file in mailrequest.Attachments)
                {
                    Console.WriteLine($"Attachment: {file.FileName}");
                }
            }

            // You can return a Task.FromResult if needed
            return Task.CompletedTask;
        }
    }


}
