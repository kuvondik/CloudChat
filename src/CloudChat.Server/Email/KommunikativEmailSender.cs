using CloudChat.Core;
using System.Threading.Tasks;
using static Dna.FrameworkDI;

namespace CloudChat.Server
{
    public static class KommunikativEmailSender
    {
        public static async Task<SendEmailResponse> SendUserEmailVerificationAsync(string displayName, string email, string verificationUrl)
        {
            return await DI.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                FromName = Configuration["KommunikativIlovaSettings:SendEmailFromName"],
                FromEmail = Configuration["KommunikativIlovaSettings:SendEmailFromEmail"],
                ToName = displayName,
                ToEmail = email,
                IsHTML = true,
                Subject = "Verify Your Email - Cloud Chat",
            },
            "Verify Email",
            $"Hi, {displayName ?? "stranger"}",
            "Thanks for creating an account with us.<br/>To continue please verify your email.",
            "Verify your email",
            verificationUrl);
        }
    }
}
