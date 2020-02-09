using System;
using System.Runtime.CompilerServices;

namespace CloudChat.Core
{
    public interface ILogFactory
    {
        event Action<(string Message, LogLevel Level)> NewLog;
        bool IncludeLogOriginDetails { get; set; }
        LogFactoryLevel LogOutputLevel { get; set; }
        void AddLogger(ILogger logger);
        void RemoveLogger(ILogger logger);
        void Log(string message, LogLevel level = LogLevel.Informative, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);
    }
}
