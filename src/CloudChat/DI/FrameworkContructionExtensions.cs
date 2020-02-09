using Dna;
using CloudChat.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CloudChat
{
    public static class FrameworkConstructionExtensions
    {
        public static FrameworkConstruction AddKommunikativIlovaViewModels(this FrameworkConstruction construction)
        {
            construction.Services.AddSingleton<ApplicationViewModel>();
            construction.Services.AddSingleton<SettingsViewModel>();
            construction.Services.AddSingleton<AccountInfoViewModel>();
            construction.Services.AddSingleton<ChatListViewModel>();
            construction.Services.AddSingleton<ChatListItemViewModel>();
            construction.Services.AddSingleton<ContactListViewModel>();
            construction.Services.AddSingleton<ChatMessageListViewModel>();
            construction.Services.AddSingleton<ChatInfoControlViewModel>();

            return construction;
        }

        public static FrameworkConstruction AddKommunikativIlovaClientServices(this FrameworkConstruction construction)
        {
            construction.Services.AddTransient<ITaskManager, BaseTaskManager>();
            construction.Services.AddTransient<IFileManager, BaseFileManager>();
            construction.Services.AddTransient<IUIManager, UIManager>();

            return construction;
        }
    }
}