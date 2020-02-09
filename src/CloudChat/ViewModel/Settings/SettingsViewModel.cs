using System.Windows.Input;
using static CloudChat.DI;
namespace CloudChat
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsOptionViewModel Account { get; set; }
        public SettingsOptionViewModel Chat { get; set; }
        public SettingsOptionViewModel Privacy { get; set; }
        public SettingsOptionViewModel DataUsage { get; set; }
        public SettingsOptionViewModel Share { get; set; }
        public SettingsOptionViewModel Help { get; set; }

        public string Name { get; set; }

        public ICommand CloseCommand { get; set; }
        public ICommand OpenCommand { get; set; }



        #region Constructor
        public SettingsViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            OpenCommand = new RelayCommand(Open);
            Privacy = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/lock.png",
                Title = "Privacy",
            };
            DataUsage = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/data.png",
                Title = "Data and Storage Usage",
            };
            Chat = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/chat.png",
                Title = "Chats",
            };
            Share = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/heart.png",
                Title = "Tell a friend",
            };
            Help = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/help.png",
                Title = "Help",
            };
            Account = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/person.png",
                Title = "Account",
            };
        }
        #endregion

        #region Command Methods

        public void Close()
        {

            ViewModelApplication.AddContactFormVisible = false;
            ViewModelApplication.ChatInfoControlVisible = false;
            if (ContactList.SelectedBefore != null)
            {
                ContactList.SelectedBefore.IsSelected = false;
                ContactList.SelectedBefore = null;
            }

            if (AccountInfo.AccountInfoVisible)
                AccountInfo.AccountInfoVisible = false;
            else
            {
                ViewModelApplication.SettingsControlVisible = false;
                ViewModelApplication.DimmableOverlayVisible = false;
            }
        }
        public void Open()
        {
            ViewModelApplication.SettingsControlVisible = true;
            ViewModelApplication.DimmableOverlayVisible = true;
        }
        #endregion
    }
}
