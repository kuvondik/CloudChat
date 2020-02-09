using Dna;
using CloudChat.Core;

namespace CloudChat
{
    public static class DI
    {
        public static IUIManager UI => Framework.Service<IUIManager>();
        public static ApplicationViewModel ViewModelApplication => Framework.Service<ApplicationViewModel>();
        public static SettingsViewModel ViewModelSettings => Framework.Service<SettingsViewModel>();
        public static AccountInfoViewModel AccountInfo => Framework.Service<AccountInfoViewModel>();
        public static ChatListViewModel ChatList => Framework.Service<ChatListViewModel>();
        public static ContactListViewModel ContactList => Framework.Service<ContactListViewModel>();
        public static ChatMessageListViewModel ChatMessageList => Framework.Service<ChatMessageListViewModel>();
        public static ChatInfoControlViewModel ChatInfo => Framework.Service<ChatInfoControlViewModel>();
        public static IClientDataStore ClientDataStore => Framework.Service<IClientDataStore>();
    }
}
