using Microsoft.Extensions.Configuration;
using SendEmail.Services;
using SendEmail.Settings;
using Serilog;
using System.Text.RegularExpressions;
using GemBox.Document;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ReportService>();

builder.Services.AddTransient<IMailService, MailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/report", (JsonDocument doc, HttpContext context) =>
{
    ComponentInfo.SetLicense("FREE-LIMITED-KEY");

    var templateDirectory = @".\Templates";
    var docRoot = doc.RootElement;

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
        Console.WriteLine(current.ToString());
        document.Content.Replace(inWordSomewhere, current.ToString());
    }

    document.Content.Replace(new Regex("{Date}", RegexOptions.IgnoreCase),
        DateTime.Today.ToLongDateString());

    var pdfSaveOptions = new PdfSaveOptions() { ImageDpi = 220 };

    using var pdfStream = new MemoryStream();
    var memoryStream = new MemoryStream();

    pdfStream.CopyTo(memoryStream);
    memoryStream.Position = 0;

    document.Save(memoryStream, pdfSaveOptions);

    return Results.File(memoryStream, "application/pdf", "report.pdf");
})

.WithName("Report")

.WithOpenApi();


app.Run();
