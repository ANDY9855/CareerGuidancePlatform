namespace CareerGuidancePlatform.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody);
    }
}
