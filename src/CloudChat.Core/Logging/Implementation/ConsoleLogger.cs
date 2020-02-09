using System;

namespace CloudChat.Core
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, LogLevel level)
        {
            var consoleOldColor = Console.ForegroundColor;
            var consoleColor = ConsoleColor.White;

            switch (level)
            {
                case LogLevel.Debug:
                    consoleColor = ConsoleColor.Blue;
                    break;
                case LogLevel.Error:
                    consoleColor = ConsoleColor.Red;
                    break;
                case LogLevel.Informative:
                    consoleColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Success:
                    consoleColor = ConsoleColor.Green;
                    break;
                case LogLevel.Verbose:
                    consoleColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Warning:
                    consoleColor = ConsoleColor.DarkYellow;
                    break;
            }
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
