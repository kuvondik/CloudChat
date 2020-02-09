using CloudChat.Core;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using static Dna.WebRequests;
using static CloudChat.DI;

namespace CloudChat
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool LoginIsRunning { get; set; }
        public bool Busy { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand LoginCommand { get; set; }

        public ICommand ToRegisterPageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginViewModel()
        {
            //Create commands
            LoginCommand = new RelayParameterizedCommand(async (parameter) => await LoginAsync(parameter));
            ToRegisterPageCommand = new RelayCommand(async () => await RegisterAsync());
        }

        #endregion

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="paramter"> The <see cref="SecureString"/> passed in from the view for the users password </param>
        /// <returns></returns>
        public async Task LoginAsync(object parameter)
        {
            Busy = true;

            await RunCommandAsync(() => LoginIsRunning, async () =>
            {
                var result = await PostAsync<ApiResponse<UserProfileDetailsApiModel>>(
                    RouteHelpers.GetAbsoluteUrl(ApiRoutes.Login),
                    new LoginCredentialsApiModel
                    {
                        UsernameOrEmail = Email,
                        Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                    });

                //If the response has an error...
                if (await result.DisplayErrorIfFailedAsync("Login Failed"))
                    //We are done
                    return;
                //successfully loged in... now get users data
                var loginResult = result.ServerResponse.Response;

                // Let the application view model handle what happens
                // with the successful login
                await ViewModelApplication.HandleSucccessfulLoginAsync(loginResult);
                ViewModelApplication.IsLoggedOut = false;
            })
            .ContinueWith(t =>
            {
                Busy = false;
                ChatList.DownloadChatList();
                ChatList.CheckNewChat();
                ContactList.DownloadContacts();
            });

        }

        public async Task RegisterAsync()
        {
            ViewModelApplication.GoToPage(ApplicationPage.Register);
            await Task.Delay(1);
        }
    }
}