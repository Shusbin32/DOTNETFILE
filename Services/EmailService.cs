using System.Net;
using System.Net.Mail;

namespace LoginSystem.Services;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            using var client = new SmtpClient();
            
            // Configure these values in appsettings.json
            client.Host = _configuration["EmailSettings:SmtpServer"];
            client.Port = int.Parse(_configuration["EmailSettings:Port"]);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]
            );

            _logger.LogInformation($"Sending email using SMTP Server: {client.Host}:{client.Port}");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Email sent successfully to {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {email}");
            throw;
        }
    }
} 