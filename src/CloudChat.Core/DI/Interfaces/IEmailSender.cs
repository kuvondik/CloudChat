using System.Threading.Tasks;

namespace CloudChat.Core
{
    public interface IEmailSender
    {
        Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details);
    }
}
