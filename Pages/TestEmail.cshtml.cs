using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystem.Services;

namespace LoginSystem.Pages;

public class TestEmailModel : PageModel
{
    private readonly IEmailService _emailService;
    private readonly ILogger<TestEmailModel> _logger;

    public TestEmailModel(IEmailService emailService, ILogger<TestEmailModel> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [TempData]
    public string Message { get; set; }

    [TempData]
    public bool Success { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(string email)
    {
        try
        {
            await _emailService.SendEmailAsync(
                email,
                "Test Email",
                "This is a test email from your ASP.NET Core application.");

            Success = true;
            Message = "Test email sent successfully! Check your Mailtrap.io inbox.";
            _logger.LogInformation($"Test email sent to {email}");
        }
        catch (Exception ex)
        {
            Success = false;
            Message = $"Error sending email: {ex.Message}";
            _logger.LogError(ex, $"Error sending test email to {email}");
        }

        return RedirectToPage();
    }
} 