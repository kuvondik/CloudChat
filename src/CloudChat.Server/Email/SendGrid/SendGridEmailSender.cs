using Dna;
using CloudChat.Core;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CloudChat.Server
{
    public class SendGridEmailSender : IEmailSender
    {
        public async Task<SendEmailResponse> SendEmailAsync(SendEmailDetails details)
        {
            var apiKey = Framework.Construction.Configuration["SendGridKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(details.FromEmail, details.FromName);
            var to = new EmailAddress(details.ToEmail, details.ToName);
            var msg = MailHelper.CreateSingleEmail(from,
                                                   to,
                                                   details.Subject,
                                                   details.IsHTML ?
                                                   null : details.Content,
                                                   details.IsHTML ?
                                                   details.Content : null);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                return new SendEmailResponse();

            try
            {
                var bodyResult = await response.Body.ReadAsStringAsync();
                var sendGridResponse = JsonConvert.DeserializeObject<SendGridResponse>(bodyResult);

                var errorResponse = new SendEmailResponse
                {
                    Errors = sendGridResponse?.Errors.Select(f => f.Message).ToList()
                };


                if (errorResponse.Errors == null || errorResponse.Errors.Count == 0)
                    errorResponse.Errors = new List<string>(new[] { "Unknown error from email sending service. Please contact chat service..." });

                return errorResponse;
            }

            catch
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                return new SendEmailResponse
                {
                    Errors = new List<string>(new[] { "Unknown error occured..." })
                };
            }
        }

    }
}
