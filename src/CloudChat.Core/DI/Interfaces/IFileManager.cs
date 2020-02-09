using System.Threading.Tasks;

namespace CloudChat.Core
{
    public interface IFileManager
    {
        string NormalizePath(string path);
        string ResolvePath(string path);
        Task WriteTextToFileAsync(string text, string path, bool append = false);
    }
}
