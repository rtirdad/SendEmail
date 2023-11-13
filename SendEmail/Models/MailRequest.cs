namespace SendEmail.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }

        public string ToDisplayName { get; set; }

        public string FromDisplayName { get; set; }

        public string FromMail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IFormFileCollection? Attachments { get; set; }
    }
}
