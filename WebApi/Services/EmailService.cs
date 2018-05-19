using WebApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task SendEmailAsync(string email, string subject, string message, string htmlMessage)
        {
            var client = new SendGridClient(_configuration["Email:SendGridAPIKey"]);
            
            var from = new EmailAddress("verify@ideashare.com", "IdeaShare corp.");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, htmlMessage);
            
            await client.SendEmailAsync(msg);
        }
    }
}