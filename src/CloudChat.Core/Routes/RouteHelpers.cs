using static Dna.FrameworkDI;

namespace CloudChat.Core
{
    public static class RouteHelpers
    {
        public static string GetAbsoluteUrl(string relativeUrl)
        {
            var host = Configuration["KommunikativIlovaServer:HostUrl"];

            if (string.IsNullOrEmpty(relativeUrl))
                return host;
            if (!relativeUrl.StartsWith("/"))
                relativeUrl = $"/{relativeUrl}";
            return host + relativeUrl;
        }
    }
}
