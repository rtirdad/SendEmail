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
            email.From.Add(new MailboxAddress(mailrequest.FromDisplayName, mailrequest.FromMail));
            //email.Sender = MailboxAddress.Parse(mailrequest.FromAppPassword);
            email.To.Add(new MailboxAddress(mailrequest.ToDisplayName, mailrequest.ToEmail));

            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();

            if (mailrequest.Attachments != null)
            {
                /*/*byte[] fileBytes;
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
                }*/
                using (var ms = new MemoryStream())
                {
                    await mailrequest.Attachments.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    var attachment = new MimePart(mailrequest.Attachments.ContentType)
                    {
                        Content = new MimeContent(ms),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = mailrequest.Attachments.FileName
                    };
                    builder.Attachments.Add(attachment);
                }
            }
            builder.HtmlBody = mailrequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
