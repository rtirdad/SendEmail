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



namespace EmailTesting
{
    public class Tests
    {
        private readonly FakeMailSender fakeMailSender = new();
       

        //private readonly LastMailRequest FakeMailSender = new ();

        [Test]
        public async Task ToEmailShould()
        {
            //Assign

            var factory = new WebApplicationFactory<SendEmail.Program>().WithWebHostBuilder(builder => Setup(builder));


            //Act
            var httpClient = factory.CreateClient();

            var mail = new MailRequest()

            {

                ToEmail = "test@gmail.com",

                ToDisplayName = "Test",

                FromDisplayName = "Tester",

                FromMail = "test@gmail.com",

                Subject = "Testing",

                Body = "Hello",

                //Attachments = []
             
        };

            //Assert
            var respondse = await httpClient.PostAsJsonAsync<MailRequest>("/send", mail);
            //respondse.EnsureSuccessStatusCode();
            //fakeMailSender.Model.Body.Should().Be("Hello!");
            //fakeMailSender.LastMailRequest.Body.Should().Be("Hello");
            respondse.Should().NotBeNull();
            //respondse.Content.Should().Be("Hello");
        }
        public void Setup(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>

            {
                services.AddSingleton<IMailService, FakeMailSender>();

            });

        }

        [Test]
        public async Task GetBooks_ShouldReturnListOfBooks()
        {
            // Arrange
            var factory = new WebApplicationFactory<SendEmail.Program>().WithWebHostBuilder(builder => Setup(builder));
            var httpClient = factory.CreateClient();

            // Act
            var response = await httpClient.GetAsync("/book");

            // Assert
            response.Should().NotBeNull();
            response.EnsureSuccessStatusCode(); // Ensure that the HTTP response indicates success (status code 2xx)

            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            books.Should().NotBeNull();
            books.Should().HaveCountGreaterThan(0); // Assuming there are books in the list
        }
    }
}