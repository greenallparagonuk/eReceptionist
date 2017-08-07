using System.Threading.Tasks;

namespace eReceptionist.Services
{
    public interface ISendGridMessage
    {
         Task SendEmailAsync(string email, string subject, string message);
    }
}