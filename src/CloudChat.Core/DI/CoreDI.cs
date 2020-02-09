using Dna;

namespace CloudChat.Core
{
    public static class CoreDI
    {
        public static IFileManager FileManager => Framework.Service<IFileManager>();
        public static ITaskManager TaskManager => Framework.Service<ITaskManager>();
    }
}