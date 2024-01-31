using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using GemBox.Document;

namespace SendEmail.Services
{
    public class ReportService
    {
        public MemoryStream GenerateReport(string jsonData)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            var templateDirectory = @".\Templates";
            var docRoot = JsonDocument.Parse(jsonData).RootElement;

            var docFormat = docRoot.GetProperty("format").ToString();
            var templateName = docRoot.GetProperty("template").ToString();
            var filePath = Path.Combine(templateDirectory, $"{templateName}.docx");
            DocumentModel document = DocumentModel.Load(filePath);

            var clientData = docRoot.GetProperty("data");

            var pattern = @"\{{.*?\}}";
            Regex rg = new Regex(pattern);
            var matches = rg.Matches(document.Content.ToString());

            foreach (Match match in matches)
            {
                var inWordSomewhere = match.Value;

                var parts = inWordSomewhere.Split(new string[] { "{{", "}}", "." }, StringSplitOptions.RemoveEmptyEntries);

                var current = clientData;
                foreach (var part in parts)
                {
                    current = current.GetProperty(part);
                }

                document.Content.Replace(inWordSomewhere, current.ToString());
            }

            document.Content.Replace(new Regex("{Date}", RegexOptions.IgnoreCase),
                DateTime.Today.ToLongDateString());

            var pdfSaveOptions = new PdfSaveOptions() { ImageDpi = 220 };

            using var pdfStream = new MemoryStream();
            var memoryStream = new MemoryStream();

            document.Save(memoryStream, pdfSaveOptions);

            return memoryStream;
        }
    }
}
