using CloudChat.Core;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using static CloudChat.DI;

namespace CloudChat
{
    public class ChatInfoControlViewModel : BaseViewModel
    {
        #region Private Members
        private readonly string mLoadingText = "...";
        private string mPictureThumbnailName;
        #endregion

        #region Public Properties

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => StringHelpers.GetShortName(firstName: FirstName,
                                                          lastName: LastName,
                                                          userName: Username);
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string PictureThumbnailUrl { get; set; }
        public string PictureThumbnailName
        {
            get => mPictureThumbnailName;
            set
            {
                if (value=="")
                    return;

                mPictureThumbnailName = value;

                if (value == null)
                    return;

                var localPath = Directory.GetCurrentDirectory() + "\\Cache\\" + mPictureThumbnailName;
                if (!File.Exists(localPath))
                    CoreDI.TaskManager.Run(async () =>
                                await FTPServices.DownloadThumbAsync(mPictureThumbnailName, FileType.ProfilePicture));

                PictureThumbnailUrl = localPath;

                OnPropertyChanged(nameof(PictureThumbnailUrl));
            }
        }
        public bool SendMessageAvailable { get; set; }
        #endregion

        #region Public Commands

        public ICommand CloseCommand { get; set; }
        public ICommand SendMessageCommand { get; set; }
        #endregion

        #region Contructor

        public ChatInfoControlViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            SendMessageCommand = new RelayCommand(SendMessage);
        }
        #endregion

        public void Close()
        {
            ViewModelApplication.ChatInfoControlVisible = false;
            ViewModelApplication.DimmableOverlayVisible = false;

            if (ContactList.SelectedBefore != null)
                ContactList.SelectedBefore.IsSelected = false;
            ContactList.SelectedBefore = null;
        }

        public void Clear()
        {
            FirstName = mLoadingText;
            LastName = mLoadingText;
            Email = mLoadingText;
            PhoneNumber = mLoadingText;
            Username = mLoadingText;

            mPictureThumbnailName = null;
            PictureThumbnailUrl = null;
            PictureUrl = null;
            SendMessageAvailable = false;
        }

        public void SendMessage()
        {
            var name =StringHelpers.GetShortName(firstName: FirstName,
                                                 lastName: LastName,
                                                 userName:Username);
            var oldChat = ChatList.Items.Where(chat=>chat.Type == ConversationType.Chat &&
                                                     chat.UsernameList.Contains(Username))
                                        .FirstOrDefault();

            if (oldChat == null)
            {
                Close();
                if (ChatList.SelectedBefore != null)
                {
                    ChatList.SelectedBefore.IsSelected = false;
                    ChatList.SelectedBefore = null;
                }

                ChatMessageList.ClearChatPage();
                ChatMessageList.ReceiverUsernamesList.Add(Username);
                ChatMessageList.Title = name;

                ViewModelApplication.ChatPageVisible = true;
            }
            else
            {
                Close();
                oldChat.OpenMessage();
            }
        }
    }
}
