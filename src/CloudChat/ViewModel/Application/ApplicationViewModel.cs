using CloudChat.Core;
using System.Threading.Tasks;
using System.Windows.Input;
using static CloudChat.Core.CoreDI;
using static CloudChat.DI;

namespace CloudChat
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Private Members

        private bool mSettingsControlVisible;
        #endregion

        #region Public Properties

        public ApplicationPage CurrentPage { get; private set; }
        public bool ChatPageVisible { get; set; } = false;
        public bool SearchVisible { get; set; } = false;
        public bool AddContactFormVisible { get; set; } = false;
        public bool ContactsAreDownloaded { get; set; } = false;
        public bool ChatsAreDownloaded { get; set; } = false;
        public bool MessagesAreDownloaded { get; set; } = false;
        public bool SideMenuVisible { get; set; } = false;
        public bool SettingsControlVisible
        {
            get => mSettingsControlVisible;
            set
            {
                // If property has not changed
                if (mSettingsControlVisible == value)
                    // Ignore
                    return;

                // Set the backing field
                mSettingsControlVisible = value;

                // If the settings menu now visible...
                if (value)
                    // Reload settigs
                    TaskManager.RunAndForget(AccountInfo.LoadAsync);
            }
        }
        public bool CloudAnimationVisible { get; set; } = true;
        public bool ChatInfoControlVisible { get; set; } = false;
        public bool DimmableOverlayVisible { get; set; } = false;
        public bool IsHitTestVisible { get; set; } = true;
        public bool IsLoggedOut { get; set; } = false;

        public BaseViewModel CurrentPageViewModel { get; set; }
        public SideMenuContent CurrentSideMenuContent { get; set; }
        public bool ServerReachable { get; set; } = true;
        #endregion

        #region Public Commands

        public ICommand OpenChatsCommand { get; set; }
        public ICommand OpenGroupsCommand { get; set; }
        public ICommand OpenContactsCommand { get; set; }
        #endregion

        #region Constructor

        public ApplicationViewModel()
        {
            OpenChatsCommand = new RelayCommand(OpenChats);
            OpenGroupsCommand = new RelayCommand(OpenGroups);
            OpenContactsCommand = new RelayCommand(OpenContacts);
        }
        #endregion

        #region Public Methods

        public void OpenGroups()
        {
            CurrentSideMenuContent = SideMenuContent.Groups;
        }

        public void OpenContacts()
        {
            CurrentSideMenuContent = SideMenuContent.Contacts;
            if (!ViewModelApplication.ContactsAreDownloaded)
                ContactList.DownloadContacts();
        }

        public void OpenChats()
        {
            CurrentSideMenuContent = SideMenuContent.Chats;

            if (!ViewModelApplication.ChatsAreDownloaded)
                ChatList.DownloadChatList();
        }

        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            SettingsControlVisible = false;
            DimmableOverlayVisible = false;
            CurrentPageViewModel = viewModel;
            var different = CurrentPage != page;
            CurrentPage = page;
            CloudAnimationVisible = page != ApplicationPage.Chat;
            SideMenuVisible = !CloudAnimationVisible;

            if (SideMenuVisible)
                IsLoggedOut = false;

            if (!different)
                OnPropertyChanged(nameof(CurrentPage));
            //OnPropertyChanged(nameof(CurrentPage));
        }

        public async Task HandleSucccessfulLoginAsync(UserProfileDetailsApiModel loginResult)
        {
            await ClientDataStore.SaveLoginCredentialsAsync(loginResult.ToLoginCredentialsDataModel());
            await AccountInfo.LoadAsync();
            ViewModelApplication.GoToPage(ApplicationPage.Chat);
        }
        #endregion
    }
}