namespace SendEmail.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }

        public string ToDisplayName { get; set; }

        public string SenderDisplayName { get; set; }

        public string SenderMail { get; set; } = string.Empty;

        //public string SenderAppPassword { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IFormFileCollection? Attachments { get; set; }
    }
}
