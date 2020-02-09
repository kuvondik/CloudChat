using Dna;
using CloudChat.Core;
using CloudChat.Relational;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows;
using static Dna.FrameworkDI;
using static CloudChat.Core.CoreDI;
using static CloudChat.DI;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);
            await ApplicationSetupAsync();
            Logger.LogDebugSource("Application starting up ...");
            ViewModelApplication.GoToPage(await ClientDataStore.HasCredentialsAsync() ?
                                                                 ApplicationPage.Chat : ApplicationPage.Login);
            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        private async Task ApplicationSetupAsync()
        {
            Framework.Construct<DefaultFrameworkConstruction>()
                     .AddClientDataStore()
                     .AddKommunikativIlovaViewModels()
                     .AddFileLogger()
                     .AddKommunikativIlovaClientServices()
                     .Build();

            // Ensure the client data store
            await ClientDataStore.EnsureDataStoreAsync();

            MonitorServerStatus();

            // Load settings
            if (ViewModelApplication.CurrentPage == ApplicationPage.Chat)
                TaskManager.RunAndForget(AccountInfo.LoadAsync);
            ChatMessageList.SetSenderInfo();
        }

        private void MonitorServerStatus()
        {
            ViewModelApplication.ServerReachable = false;

            var httpWatcher = new HttpEndpointChecker(
                Configuration["KommunikativIlovaServer:HostUrl"],
                interval: 3000,
                logger: Framework.Provider.GetService<Microsoft.Extensions.Logging.ILogger>(),
                stateChangedCallback: (result) =>
                 {
                     ViewModelApplication.ServerReachable = result;
                     if (!ViewModelApplication.ContactsAreDownloaded)
                         ContactList.DownloadContacts();
                     if (!ViewModelApplication.ChatsAreDownloaded)
                         ChatList.DownloadChatList();
                 });
        }
    }
}