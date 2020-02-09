using Dna;
using CloudChat.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CloudChat
{
    public class AddContactFormViewModel : BaseViewModel
    {
        #region Public Properties

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureThumbName { get; set; }
        public bool ContactIsSaving { get; set; }
        public bool Saving { get; set; } = false;
        #endregion

        #region Public Commands

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Constructor

        public AddContactFormViewModel()
        {
            CancelCommand = new RelayParameterizedCommand(Cancel);
            SaveCommand = new RelayParameterizedCommand(parameter => Save(parameter));
        }
        #endregion

        #region Command Methods

        public void Save(object parameter)
        {
            Saving = true;
            RunCommandAsync(() => ContactIsSaving, async () =>
            {
                // Adding the new contact to the server
                var credentials = await DI.ClientDataStore.GetLoginCredentialsAsync();

                var request = await WebRequests.PostAsync<ApiResponse<ContactApiModel>>(
                    RouteHelpers.GetAbsoluteUrl(ApiRoutes.AddContact),
                    new ContactApiModel
                    {
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        OwnerUsername = credentials?.Username,
                    },
                    bearerToken: credentials?.Token);

                //If the response has an error...
                if (await request.DisplayErrorIfFailedAsync("User not found"))
                    //We are done
                    return;
                var response = request.ServerResponse.Response;

                Username = response.Username;
                FirstName = response.FirstName;
                LastName = response.LastName;
                PictureThumbName = response.PictureThumbName;

                DI.ViewModelApplication.DimmableOverlayVisible = false;
                DI.ViewModelApplication.AddContactFormVisible = false;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Adding to the ContactList UI
                    DI.ContactList.Items.Add(new ContactItemViewModel
                    {
                        Username = Username,
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        PictureThumbName = PictureThumbName,
                        Initials =StringHelpers.GetInitials(Username),
                        Rgb = new RgbValue().GetRandomRgbValue(),
                    });

                    (parameter as TextBox).Text = "";
                });
            }).ContinueWith(t =>
            {
                Saving = false;
            });
        }

        public void Cancel(object parameter)
        {
            DI.ViewModelApplication.DimmableOverlayVisible = false;
            DI.ViewModelApplication.AddContactFormVisible = false;

            (parameter as TextBox).Text = "";
        }
        #endregion
    }
}
