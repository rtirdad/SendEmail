using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendEmail.Services;
using SendEmail.Settings;
using Serilog;
using System.Collections.Generic;

namespace SendEmail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .Enrich.FromLogContext()
              .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            // Add services to the container.
            builder.Services.AddControllers();

            // Correct the namespace if necessary
            builder.Services.AddSingleton<IMailService, MailService>();
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

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

            var books = new List<Book>
            {
                new Book { Id=1, Title="Pride and Prejudice", Author="Jane Austen"},
                new Book { Id=2, Title="Macbeth", Author="William Shakespeare"},
            };

            app.MapGet("/book", () =>
            {
                return books;
            });

            app.MapGet("/book/{id}", (int id) =>
            {
                var book = books.Find(b => b.Id == id);
                if (book == null)
                    return Results.NotFound("Sorry, this book does not exist in our library");
                return Results.Ok(book);
            });

            app.MapPost("/book", (Book book) =>
            {
                books.Add(book);
                return books;
            });

            app.MapPut("/book/{id}", (Book updatedBook, int id) =>
            {
                var book = books.Find(b => b.Id == id);
                if (book == null)
                    return Results.NotFound("Sorry, this book does not exist in our library");

                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                return Results.Ok(book);
            });

            app.MapDelete("/book/{id}", (int id) =>
            {
                var book = books.Find(b => b.Id == id);
                if (book == null)
                    return Results.NotFound("Sorry, this book does not exist in our library");

                books.Remove(book);
                return Results.Ok(book);
            });

            app.Run();

        }
        class Book
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
        }
    }
}
