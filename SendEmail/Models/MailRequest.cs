﻿using System.Text.Json;

namespace SendEmail.Models
{
    public class MailRequest
    {
        public string ToEmail { get; set; }

        public string ToDisplayName { get; set; }

        public string FromDisplayName { get; set; }

        public string FromMail { get; set; }

        //public string FromAppPassword { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IFormFile? Attachments { get; set; } // Change to IFormFile

        public string JsonData { get; set; }

        public JsonElement GetJsonDataAsJsonElement() => JsonDocument.Parse(JsonData).RootElement;
    }
}
