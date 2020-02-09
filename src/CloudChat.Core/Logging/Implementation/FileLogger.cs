using System;

namespace CloudChat.Core
{
    public class FileLogger : ILogger
    {
        public string FilePath { get; set; }
        public bool LogTime { get; set; } = true;

        public FileLogger(string filePath)
        {
            FilePath = filePath;
        }

        public void Log(string message, LogLevel level)
        {
            var currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd hh:mm:ss");

            var timeLogString = LogTime ? $"[{currentTime}]" : "";
            CoreDI.FileManager.WriteTextToFileAsync($"{timeLogString}{message}{Environment.NewLine}", FilePath, append: true);
        }
    }
}
