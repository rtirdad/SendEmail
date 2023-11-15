using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SendEmail.Models;
using SendEmail.Services;
using System.Net.Http.Json;
using SendEmail;
using System.Net;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MailKit.Net.Proxy;
//using NUnit.Framework.Constraints;

namespace EmailTesting
{
    public class Tests
    {
        private readonly FakeMailSender fakeMailSender = new();
        //private readonly IMailService fakeMailService = new();
        //private Program Book;
        //private readonly LastMailRequest FakeMailSender = new ();



        [Test]
        public async Task FakeMailSender_Should_return_Content_in_MailRequest()
        //public async Task SendEmailAsync_WhenValidMailRequestIsProvidedShouldSendEmail()
        {
            // Arrange
            var factory = new WebApplicationFactory<SendEmail.Program>().WithWebHostBuilder(builder => Setup(builder));
            var httpClient = factory.CreateClient();
            var mail = new MailRequest()
            {
                ToEmail = "test@gmail.com",
                ToDisplayName = "Test",
                FromDisplayName = "Test",
                FromMail = "test@gmail.com",
                Subject = "Testing",
                Body = "Hello",
                //Attachments = new[] {}
            };

            // Act
            var response = await httpClient.PostAsJsonAsync<MailRequest>("/send", mail);

            // Assert

            //fakeMailSender.Subject.Should().Be("Testing");
            fakeMailSender.FakeMailRequest.ToDisplayName.Should().Be("Test");

            //fakeMailSender.FakeMailRequest.ToEmail.Should().Be("test@gmail.com");
            //response.Should().NotBeNull();


        }

        public void Setup(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>

            {
                services.AddSingleton<IMailService, FakeMailSender>();

            });
        }

        [Test]
        public async Task Return_Helloworld()
        {
            //Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            //Act
            var httpClient = factory.CreateClient();
            var respondse = await httpClient.GetStringAsync("hello");

            //Assert
            respondse.Should().Be("Hello World!");
        }

        [Test]
        public async Task Return_All_Books()
        {
            //Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            //Act
            var httpClient = factory.CreateClient();
            var respondse = await httpClient.GetStringAsync("/book");

            //Assert
            respondse.Should().Contain("Macbeth");
        }

        [Test]
        public async Task Create_new_book()
        {
            //Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            //Act
            var httpClient = factory.CreateClient();
            var respondse = await httpClient.PostAsJsonAsync("/book", new Program.Book()
            {
                Id = 4,
                Title = "The great Gatsby",
                Author = "F. Scott fitzgerald"
            });
            
            var content = await respondse.Content.ReadAsStringAsync();

           //Assert
           //Assert.IsTrue(respondse.StatusCode == HttpStatusCode.OK);
           Assert.AreEqual(HttpStatusCode.OK, respondse.StatusCode);
           content.Should().Contain("The great Gatsby");
            content.Should().Contain("F. Scott fitzgerald");
           respondse.Should().NotBeNull();
        }
    }
}