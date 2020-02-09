using CloudChat.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudChat
{
    public class ChatMessageItemImageAttachmentViewModel : BaseViewModel
    {
        #region Private Members
        private string mFileThumbnailName;
        private string mFileName;
        #endregion

        public string Title { get; set; }
        public long FileSize { get; set; }
        public bool Downloading { get; set; } = false;
        public string FileUrl { get; set; }
        public string FileThumbnailPath { get; set; }
        public string FileThumbnailName
        {
            get => mFileThumbnailName;
            set
            {
                if (value == mFileThumbnailName || value == "")
                    return;

                mFileThumbnailName = value;

                if (value == null)
                    return;

                var localPath = Directory.GetCurrentDirectory() + "\\Cache\\" + mFileThumbnailName;

                CoreDI.TaskManager.Run(async () =>
                {
                    if (!File.Exists(localPath))
                        await FTPServices.DownloadThumbAsync(FileThumbnailName, FileType.Attachment);
                    FileThumbnailPath = localPath;
                });

                OnPropertyChanged(nameof(FileThumbnailPath));
            }
        }
        public string FileName
        {
            get => mFileName;
            set
            {
                if (value == mFileName)
                    return;
                mFileName = value;
                var fileUrl = Directory.GetCurrentDirectory() + "\\Cache\\" + FileName;
                if (File.Exists(fileUrl))
                {
                    FileUrl = fileUrl;
                    OnPropertyChanged(nameof(FileUrl));
                    OnPropertyChanged(nameof(IsDownloaded));
                }
            }
        }
        public string LocalFilePath { get; set; }
        public bool IsSentByMe { get; set; }

        public bool IsDownloaded => FileUrl != null;
        public bool IsDownloading { get; set; }

        //public bool ImageLoaded => FileUrl != null;

        public ICommand DownloadFileCommand { get; set; }
        public ICommand CancelDownloadFileCommand { get; set; }


        #region Constructor
        public ChatMessageItemImageAttachmentViewModel()
        {
            DownloadFileCommand = new RelayCommand(async () => await DownloadFile());
            CancelDownloadFileCommand = new RelayCommand(CancelDownload);
        }
        #endregion



        public void CancelDownload()
        {
            throw new NotImplementedException();
        }

        public async Task DownloadFile()
        {
            var localPath = Directory.GetCurrentDirectory() + "\\Cache\\" + FileName;
            Downloading = true;

            await RunCommandAsync(() => IsDownloading, async () =>
            {
                if (!File.Exists(localPath))
                    await FTPServices.DownloadAsync(FileName, FileType.Attachment);
            }).ContinueWith(t =>
            {
                FileUrl = localPath;
                Downloading = false;
                OnPropertyChanged(nameof(FileUrl));
            });
        }
    }
}