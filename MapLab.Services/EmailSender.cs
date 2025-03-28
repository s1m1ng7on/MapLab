using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MapLab.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailSender(IConfiguration configuration)
        {
            _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            _senderEmail = configuration["SendGrid:SenderEmail"];
            _senderName = configuration["SendGrid:SenderName"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_senderEmail, _senderName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Email sending failed.");
            }
        }
    }

}
