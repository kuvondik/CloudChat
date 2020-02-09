using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CloudChat.Core
{
    public class BaseFileManager : IFileManager
    {
        public async Task WriteTextToFileAsync(string text, string path, bool append = false)
        {
            //TODO: Add exception catching

            await AsyncAwaiter.AwaitAsync(nameof(BaseFileManager) + path, async () =>
                {
                    //TODO: Add IoC.Task.Run that logs to logger on failure

                    path = NormalizePath(path);
                    path = ResolvePath(path);

                    // Run the synchronous file access as a new task
                    await CoreDI.TaskManager.Run(() =>
                    {
                        //Write a log message to file
                        using (var fileStream = (TextWriter)new StreamWriter(File.Open(path, append ? FileMode.Append : FileMode.Create)))
                            fileStream.Write(text);

                    });
                });
        }
        public string NormalizePath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return path = path.Replace('/', '\\').Trim();
            else
                return path = path.Replace('/', '\\').Trim();
        }
        public string ResolvePath(string path)
        {
            return Path.GetFullPath(path);
        }
    }
}
