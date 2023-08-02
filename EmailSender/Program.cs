using EmailSender.Data;
using EmailSender.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace EmailSender;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddScoped<IEmailSenderService>(_ => new EmailSenderService(_.GetService<EmailDbContext>(), "", _.GetService<ILogger<EmailSenderService>>()));
        builder.Services.AddDbContext<EmailDbContext>(options => options.UseInMemoryDatabase("InMemoryEmailDatabase"));
        builder.Services.AddHangfire(configuration => configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

