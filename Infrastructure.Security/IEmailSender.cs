using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public interface IEmailSender
    {
        Task Execute(string apiKey, string subject, string message, string email);
        Task SendEmailAsync(string email, string subject, string message);
    }
}