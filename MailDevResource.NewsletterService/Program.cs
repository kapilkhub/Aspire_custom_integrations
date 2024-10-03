using MailKit.Client;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.AddMailKitClient("maildev");

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapPost("/subscribe",
	async (MailKitClientFactory factory, string email) =>
	{
		ISmtpClient client = await factory.GetSmtpClientAsync();

		using var message = new MailMessage("newsletter@yourcompany.com", email)
		{
			Subject = "Welcome to our newsletter!",
			Body = "Thank you for subscribing to our newsletter!"
		};

		await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
	});

app.MapPost("/unsubscribe",
	async (MailKitClientFactory factory, string email) =>
	{
		ISmtpClient client = await factory.GetSmtpClientAsync();

		using var message = new MailMessage("newsletter@yourcompany.com", email)
		{
			Subject = "You are unsubscribed from our newsletter!",
			Body = "Sorry to see you go. We hope you will come back soon!"
		};

		await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
	});

app.Run();

