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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace EmailTesting
{
    public class Tests
    {
       private readonly FakeMailSender fakeMailSender = new();

        [Test]
        public async Task FakeMailSender_Should_return_Content_in_MailRequest()
        {
            // Arrange
            var factory = new WebApplicationFactory<SendEmail.Program>().WithWebHostBuilder(builder => Setup(builder));
            var httpClient = factory.CreateClient();

            var mailInfo = new MailRequest()
            {
                ToEmail = "test@gmail.com",
                ToDisplayName = "Test",
                FromDisplayName = "Test",
                FromMail = "test@gmail.com",
                Subject = "Testing",
                Body = "Hello",
                Attachments = {}
            };

            // Act
            //var response = await httpClient.PostAsJsonAsync<FakeMailSender>("/send" , mailInfo);
            var response = await httpClient.PostAsJsonAsync<MailRequest>("/send", mailInfo);
            await fakeMailSender.SendEmailAsync(mailInfo);

            // Assert
            fakeMailSender.MailRequest.Should().BeEquivalentTo(mailInfo);
            fakeMailSender.MailRequest.ToDisplayName.Should().Be("Test");
            fakeMailSender.MailRequest.Body.Should().Be("Hello");   
            fakeMailSender.MailRequest.ToEmail.Should().Be("test@gmail.com");
            fakeMailSender.MailRequest.Attachments.Should().BeNull();
            response.Should().NotBeNull();
        }

        public void Setup(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IMailService>(fakeMailSender);
            });
        }

        [Test]
        public async Task Return_Helloworld()
        {
            //Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            //Act
            var httpClient = factory.CreateClient();
            var response = await httpClient.GetStringAsync("hello");

            //Assert
            response.Should().Be("Hello World!");
        }

        [Test]
        public async Task Return_All_Books()
        {
            //Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            //Act
            var httpClient = factory.CreateClient();
            var response = await httpClient.GetStringAsync("/book");

            //Assert
            response.Should().Contain("Macbeth");
        }

        [Test]
        public async Task Create_new_book()
        {
            //Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            //Act
            var httpClient = factory.CreateClient();
            var response = await httpClient.PostAsJsonAsync("/book", new Program.Book()
            {
                Id = 4,
                Title = "The great Gatsby",
                Author = "F. Scott fitzgerald"
            });
            
            var content = await response.Content.ReadAsStringAsync();

           //Assert
           Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
           content.Should().Contain("The great Gatsby");
            content.Should().Contain("F. Scott fitzgerald");
           response.Should().NotBeNull();
        }

        [Test]
        public async Task Update_book_Details()
        {
            // Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            // Act
            var httpClient = factory.CreateClient();

            var idToUpdate = 2;
            var response = await httpClient.PutAsJsonAsync($"/book/{idToUpdate}", new Program.Book()
            {
                Id = idToUpdate,
                Title = "Hamlet",
                Author = "William Shakespeare"
            });

            var content = await response.Content.ReadAsStringAsync();

            // Assert
            content.Should().Contain("Hamlet");
            content.Should().Contain("William Shakespeare");
        }

        [Test]
        public async Task DeleteBook()
        {
            // Assign
            var factory = new WebApplicationFactory<SendEmail.Program>();

            // Act
            var httpClient = factory.CreateClient();

            var idToDelete = 2;
            var response = await httpClient.PutAsJsonAsync($"/book/{idToDelete}", new Program.Book());

            var content = await response.Content.ReadAsStringAsync();

            // Assert
            content.Should().NotContain("Pride and Prejudice");
            content.Should().NotContain("Jane Austen");
        }
    }
}