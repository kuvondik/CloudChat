using CloudChat.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CloudChat.Server
{
    public static class EmailTemplateSenderExtensions
    {
        public static IServiceCollection AddEmailTemplateSender(this IServiceCollection services)
        {
            services.AddTransient<IEmailTemplateSender, EmailTemplateSender>();
            return services;
        }
    }
}
