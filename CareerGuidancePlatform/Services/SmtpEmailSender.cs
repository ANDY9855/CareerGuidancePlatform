using System.Net;
using System.Net.Mail;

namespace CareerGuidancePlatform.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody)
        {
            var settings = _config.GetSection("EmailSettings");
            var smtpServer = settings["SmtpServer"]!;
            var smtpPort   = int.Parse(settings["SmtpPort"]!);
            var senderName  = settings["SenderName"]!;
            var senderEmail = settings["SenderEmail"]!;
            var password    = settings["Password"]!;

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl   = true
            };

            var mail = new MailMessage
            {
                From       = new MailAddress(senderEmail, senderName),
                Subject    = subject,
                Body       = htmlBody,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(toEmail, toName));

            await client.SendMailAsync(mail);
        }
    }
}
