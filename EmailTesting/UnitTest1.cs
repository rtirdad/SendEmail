using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SendEmail.Models;
using SendEmail.Services;
using System.Net.Http.Json;

namespace EmailTesting
{
    public class Tests
    {
        //private readonly FakeMailSender = new ();

        [Test]
        public async Task toEmailShould()
        {
            //Assign

            var factory = new WebApplicationFactory<SendEmail.Program>().WithWebHostBuilder(builder => Setup(builder));


            //Act
            var httpClient = factory.CreateClient();

            var mail = new MailRequest()

            {

                ToEmail = "ronat20003@gmail.com",

                ToDisplayName = "rona",

                FromDisplayName = "Rona",

                FromMail = "ronat20003@gmail.com",

                Subject = "Testing",

                Body = "Hello",

                //Attachments = []

            };

            //Assert
            var respondse = await httpClient.PostAsJsonAsync<MailRequest>("/send", mail);
            respondse.EnsureSuccessStatusCode();
            
        }
        public void Setup(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>

            {
                services.AddSingleton<IMailSender, FakeMailSender>();

            });

        }

    }
}