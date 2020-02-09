using System.Diagnostics;

namespace CloudChat.Core
{
    public class DebugLogger : ILogger
    {
        public void Log(string message, LogLevel level)
        {
            var category = default(string);
            switch (level)
            {
                case LogLevel.Debug:
                    category = "Information";
                    break;
                case LogLevel.Error:
                    category = "error";
                    break;
                case LogLevel.Success:
                    category = "-----";
                    break;
                case LogLevel.Verbose:
                    category = "verbose";
                    break;
                case LogLevel.Warning:
                    category = "warning";
                    break;
            }
            Debug.WriteLine(message, category);
        }
    }
}
