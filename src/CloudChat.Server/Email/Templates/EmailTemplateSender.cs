using CloudChat.Core;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloudChat.Server
{
    public class EmailTemplateSender : IEmailTemplateSender
    {
        public async Task<SendEmailResponse> SendGeneralEmailAsync(SendEmailDetails details, string title, string content1, string content2, string buttonText, string buttonUrl)
        {
            var templateText = default(string);
            using (var reader = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("CloudChat.Server.Email.Templates.GeneralTemplate.htm"), Encoding.UTF8))
            {
                templateText = await reader.ReadToEndAsync();
            }

            templateText = templateText.Replace("--Title--", title).
                                        Replace("--Content1--", content1).
                                        Replace("--Content2--", content2).
                                        Replace("--ButtonText--", buttonText).
                                        Replace("--ButtonUrl--", buttonUrl);

            details.Content = templateText;

            return await DI.EmailSender.SendEmailAsync(details);
        }
    }
}
