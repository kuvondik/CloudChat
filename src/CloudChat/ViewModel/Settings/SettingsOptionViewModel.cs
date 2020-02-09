using System.Windows.Input;
using static CloudChat.DI;

namespace CloudChat
{
    public class SettingsOptionViewModel : BaseViewModel
    {
        public string IconPath { get; set; }
        public string Title { get; set; }
        public ICommand AccountInfoCommand { get; set; }

        public SettingsOptionViewModel()
        {
            AccountInfoCommand = new RelayCommand(AccountInfos);
        }

        public void AccountInfos()
        {
            AccountInfo.AccountInfoVisible = true;
        }
    }
}
