using System.Threading.Tasks;

namespace CloudChat.Core
{
    public interface IEmailTemplateSender
    {
        Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl);
    }
}
