using CloudChat.Core;
using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using static CloudChat.DI;

namespace CloudChat
{
    /// <summary>
    /// The View Model for a login screen
    /// </summary>
    public class RegisterViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The Email of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The Username of the user
        /// </summary>
        public string Username { get; set; }

        public SecureString Password { get; set; }

        public SecureString ConfirmPassword { get; set; }

        /// <summary>
        /// A flag indicating if the login command is running
        /// </summary>
        public bool RegisterIsRunning { get; set; }
        public bool Busy { get; set; } = false;

        #endregion

        #region Commands

        /// <summary>
        /// The command to login
        /// </summary>
        public ICommand RegisterCommand { get; set; }

        public ICommand ToLoginPageCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public RegisterViewModel()
        {
            //Create commands
            RegisterCommand = new RelayParameterizedCommand(async (paramter) => await RegisterAsync(paramter));
            ToLoginPageCommand = new RelayCommand(async () => await LoginAsync());
        }

        #endregion

        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="paramter"> The <see cref="SecureString"/> passed in from the view for the users password </param>
        /// <returns></returns>
        public async Task RegisterAsync(object parameter)
        {
            Busy = true;
            await RunCommandAsync(() => RegisterIsRunning, async () =>
            {
                if (Password.Unsecure() != ConfirmPassword.Unsecure())
                {
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Password Mismatch",
                        Message = "Please, confirm password",
                    });

                    return;
                }

                var result = await Dna.WebRequests.PostAsync<ApiResponse<RegisterResultApiModel>>(
                    RouteHelpers.GetAbsoluteUrl(ApiRoutes.Register),
                    new RegisterCredentialsApiModel
                    {
                        Username = Username,
                        Email = Email,
                        Password = (parameter as IHavePassword).SecurePassword.Unsecure()
                    });

                //If the response has an error...
                if (await result.DisplayErrorIfFailedAsync("Register Failed"))
                    //We are done
                    return;

                await CoreDI.TaskManager.Run(async () =>
                {
                    var loginResult = result.ServerResponse.Response;
                    await ViewModelApplication.HandleSucccessfulLoginAsync(loginResult);
                });
                ViewModelApplication.IsLoggedOut = false;
            })
            .ContinueWith(t =>
            {
                Busy = false;
                ChatList.CheckNewChat();
            });
        }

        public async Task LoginAsync()
        {
            ViewModelApplication.GoToPage(ApplicationPage.Login);
            await Task.Delay(1);
        }
    }
}