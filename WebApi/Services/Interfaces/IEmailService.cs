using System.Threading.Tasks;

namespace WebApi.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message, string htmlMessage);
    }
}