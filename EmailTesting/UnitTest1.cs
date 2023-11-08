using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Testing;
using SendEmail.Models;
using SendEmail.Services;
using System.Net.Http.Json;

namespace EmailTesting
{
    public class Tests
    {
      //private readonly MailSender _sender;

        [Test]
        public async Task Test1()
        {
            //Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();


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
            var respondse = httpClient.PostAsJsonAsync<MailRequest>("/send", mail);
            respondse.Status.Equals(404);
 
        }
    }
}