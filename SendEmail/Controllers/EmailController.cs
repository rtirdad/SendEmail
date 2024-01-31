using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Models;
using SendEmail.Services;
using System;
using System.Threading.Tasks;
using Serilog;
using System.Text.Json;

namespace SendEmail.Controllers
{
    /*[Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IMailService mailService, ILogger<EmailController> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                _logger.LogInformation("Email sent successfully :)");
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured, email send unsuccessfully :(");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }
    }*/

    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly ILogger<EmailController> _logger;
        private readonly ReportService _reportService;

        public EmailController(IMailService mailService, ILogger<EmailController> logger, ReportService reportService)
        {
            _mailService = mailService;
            _logger = logger;
            _reportService = reportService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                // Convert JsonData from MailRequest to JsonDocument
                var jsonDataDoc = JsonDocument.Parse(request.JsonData);

                // Generate PDF report from JsonData using ReportService
                var reportStream = _reportService.GenerateReport(jsonDataDoc);

                // Attach the report to the email
                var attachments = new List<IFormFile> { request.Attachments };

                // Send the email with the attached report
                await _mailService.SendEmailAsync(request, attachments);

                _logger.LogInformation("Email sent successfully :)");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred, email send unsuccessfully :(");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred. The Mail could not be sent.");
            }
        }


    }

}

