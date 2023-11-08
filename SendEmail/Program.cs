using Microsoft.Extensions.Configuration;
using SendEmail.Services;
using SendEmail.Settings;
using Serilog;

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

            app.Run();
        }
    }
}
