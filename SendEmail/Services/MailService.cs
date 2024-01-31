using System.IO;
using System.Threading.Tasks;
using GemBox.Document;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using SendEmail.Models;
using SendEmail.Settings;

namespace SendEmail.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ReportService _reportService;

        public MailService(IOptions<MailSettings> options, ReportService reportService)
        {
            _mailSettings = options.Value;
            _reportService = reportService;
        }

        public async Task SendEmailAsync(MailRequest mailrequest)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(mailrequest.FromDisplayName, _mailSettings.Mail));
            email.To.Add(new MailboxAddress(mailrequest.ToDisplayName, mailrequest.ToEmail));

            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();

            var reportStream = _reportService.GenerateReport(mailrequest.JsonData);
            builder.Attachments.Add("report.pdf", reportStream.ToArray(), ContentType.Parse("application/pdf"));

            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
