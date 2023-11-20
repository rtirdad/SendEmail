using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendEmail.Models;
using SendEmail.Services;
using System;
using System.Threading.Tasks;
using Serilog;

namespace SendEmail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly FakeMailSender _fakeSender;
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
                //await _mailService.SendEmailAsync(request);
                await _fakeSender.SendEmailAsync(request);
                _logger.LogInformation("Email sent successfully :)");
                return Ok();

            }
            catch (Exception)
            {
                _logger.LogError("An error occured, email send unsuccessfully :(");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }
    }
}
