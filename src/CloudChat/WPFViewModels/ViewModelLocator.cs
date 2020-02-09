using static CloudChat.DI;
namespace CloudChat
{
    public class ViewModelLocator
    {
        #region Singleton
        public static ViewModelLocator Instance { get; set; } = new ViewModelLocator();
        #endregion
        public ApplicationViewModel ApplicationViewModel => ViewModelApplication;
        public SettingsViewModel SettingsViewModel => ViewModelSettings;
        public AccountInfoViewModel AccountInfoViewModel => AccountInfo;
    }
}