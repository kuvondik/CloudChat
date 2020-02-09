using CloudChat.Server;
using Microsoft.Extensions.DependencyInjection;

namespace CloudChat.Core
{
    public static class SendGridEextensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            return services;
        }
    }
}
