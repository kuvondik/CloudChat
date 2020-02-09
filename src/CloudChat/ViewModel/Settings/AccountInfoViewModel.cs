using Dna;
using CloudChat.Core;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using static Dna.FrameworkDI;
using static CloudChat.DI;

namespace CloudChat
{
    public class AccountInfoViewModel : BaseViewModel
    {
        #region Private Member
        private readonly string initialPath = Directory.GetCurrentDirectory() + "\\Cache\\";
        private const string mLoadingText = "...";
        #endregion

        #region Public Properties
        public string PasswordChangedDate { get; set; }
        public string LastSignInDate { get; set; }
        public TextEntryViewModel FirstName { get; set; }
        public TextEntryViewModel LastName { get; set; }
        public PasswordEntryViewModel Password { get; set; }
        public TextEntryViewModel Email { get; set; }
        public TextEntryViewModel Username { get; set; }
        public bool AccountInfoVisible { get; set; } = false;
        public string ProfilePicName { get; set; }
        public string ProfilePicUri { get; set; }
        public string ProfilePicThumbName { get; set; }
        public string ProfilePicThumbUri { get; set; }
        public string LogoutButtonText { get; set; }

        #region Transactional Properties

        public bool FirstNameIsSaving { get; set; }
        public bool LastNameIsSaving { get; set; }
        public bool EmailIsSaving { get; set; }
        public bool PasswordIsChanging { get; set; }
        public bool UsernameIsSaving { get; set; }
        public bool SettingsLoading { get; set; }
        public bool LoggingOut { get; set; }

        #endregion

        #endregion

        #region Public Commands

        public ICommand LogoutCommand { get; set; }
        public ICommand ClearAllDataCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand ProfilePicEditCommand { get; set; }
        public ICommand SaveFirstNameCommand { get; set; }
        public ICommand SaveLastNameCommand { get; set; }
        public ICommand SaveUsernameCommand { get; set; }
        public ICommand SaveEmailCommand { get; set; }
        public ICommand SavePasswordCommand { get; set; }

        #endregion

        #region Constructor

        public AccountInfoViewModel()
        {
            FirstName = new TextEntryViewModel
            {
                Label = "First name",
                OriginalText = mLoadingText,
                CommitAction = SaveFirstNameAsync
            };

            LastName = new TextEntryViewModel
            {
                Label = "Last name",
                OriginalText = mLoadingText,
                CommitAction = SaveLastNameAsync
            };

            Username = new TextEntryViewModel
            {
                Label = "Username",
                OriginalText = mLoadingText,
                CommitAction = SaveUsernameAsync
            };

            Password = new PasswordEntryViewModel
            {
                Label = "Password",
                FakePassword = "********",
                CommitAction = SavePasswordAsync
            };

            Email = new TextEntryViewModel
            {
                Label = "Email",
                OriginalText = mLoadingText,
                CommitAction = SaveEmailAsync,
            };

            LogoutCommand = new RelayCommand(async () => await LogoutAsync());
            ClearAllDataCommand = new RelayCommand(ClearUserData);
            ProfilePicEditCommand = new RelayCommand(async () => await EditProfilePictureAsync());
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            SaveFirstNameCommand = new RelayCommand(async () => await SaveFirstNameAsync());
            SaveLastNameCommand = new RelayCommand(async () => await SaveLastNameAsync());
            SaveUsernameCommand = new RelayCommand(async () => await SaveUsernameAsync());
            SaveEmailCommand = new RelayCommand(async () => await SaveEmailAsync());
            SavePasswordCommand = new RelayCommand(async () => await SavePasswordAsync());

            // TODO: Get from localization
            LogoutButtonText = "Logout";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the settings view model properties based on the data in the client data store
        /// and updates through server again
        /// </summary>
        public async Task LoadAsync()
        {
            await RunCommandAsync(() => SettingsLoading, async () =>
              {
                  // Update local cached values
                  await UpdateValuesFromLocalStoreAsync();

                  var credentials = await ClientDataStore.GetLoginCredentialsAsync();
                  if (credentials == null)
                      return;

                  // Load user profile details from server
                  var result = await WebRequests.PostAsync<ApiResponse<UserProfileDetailsApiModel>>(
                        RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetUserProfile),
                        bearerToken: credentials?.Token);
                  if (await result.DisplayErrorIfFailedAsync("Settings Loading Failed"))
                      return;
                  // If it was successful...
                  if (result.Successful)
                  {
                      var userProfile = result.ServerResponse.Response;
                      if (userProfile.ProfilePictureThumbName != null && userProfile.ProfilePictureThumbName != ProfilePicThumbName)
                      {
                          ProfilePicName = userProfile.ProfilePictureName;
                          if (!File.Exists(initialPath + userProfile.ProfilePictureThumbName))
                              await FTPServices.DownloadThumbAsync(userProfile.ProfilePictureThumbName, FileType.ProfilePicture);

                          ProfilePicName = userProfile.ProfilePictureName;
                          ProfilePicThumbName = userProfile.ProfilePictureThumbName;
                          ProfilePicUri = initialPath + userProfile.ProfilePictureName;
                          ProfilePicThumbUri = initialPath + userProfile.ProfilePictureThumbName;

                          var dataModel = userProfile.ToLoginCredentialsDataModel();
                          dataModel.Token = credentials?.Token;
                          await ClientDataStore.SaveLoginCredentialsAsync(dataModel);
                          await UpdateValuesFromLocalStoreAsync();
                      }
                  }
              });
        }
        public async Task<bool> SaveFirstNameAsync()
        {
            // Lock this commmand to ignore any other requests while processing
            return await RunCommandAsync(() => FirstNameIsSaving, async () =>
            {
                // Update the First Name value on the server...
                return await UpdateUserCredentailsValueAsync(
                    // Display name
                    "First Name",
                    // Update the first name
                    (credetials) => credetials.FirstName,
                    //To new value
                    FirstName.OriginalText,
                    (apimodel, value) => apimodel.FirstName = value);
            });
        }
        public async Task<bool> SaveLastNameAsync()
        {
            // Lock this commmand to ignore any other requests while processing
            return await RunCommandAsync(() => LastNameIsSaving, async () =>
            {
                // Update the First Name value on the server...
                return await UpdateUserCredentailsValueAsync(

                    // Display name
                    "Last Name",

                    // Update the first name
                    (credetials) => credetials.LastName,

                    //To new value
                    LastName.OriginalText,
                    (apimodel, value) => apimodel.LastName = value);
            });
        }
        public async Task<bool> SaveEmailAsync()
        {
            // Lock this commmand to ignore any other requests while processing
            return await RunCommandAsync(() => EmailIsSaving, async () =>
            {
                // Update the First Name value on the server...
                return await UpdateUserCredentailsValueAsync(
                    // Display name
                    "Email",
                    // Update the first name
                    (credetials) => credetials.Email,
                    //To new value
                    Email.OriginalText,
                    (apimodel, value) => apimodel.Email = value);
            });
        }
        public async Task<bool> SaveUsernameAsync()
        {
            // Lock this commmand to ignore any other requests while processing
            return await RunCommandAsync(() => UsernameIsSaving, async () =>
            {
                // Update the First Name value on the server...
                return await UpdateUserCredentailsValueAsync(
                    // Display name
                    "Username",
                    // Update the first name
                    (credetials) => credetials.Username,
                    //To new value
                    Username.OriginalText,
                    (apimodel, value) => apimodel.Username = value);
            });
        }
        public async Task<bool> SavePasswordAsync()
        {
            // Lock this commmand to ignore any other requests while processing
            return await RunCommandAsync(() => PasswordIsChanging, async () =>
            {
                Logger.LogDebugSource("Changing password...");

                var credentials = await ClientDataStore.GetLoginCredentialsAsync();

                if (Password.NewPassword.Unsecure() != Password.ConfirmPassword.Unsecure())
                {
                    await UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Password Mismatch",
                        Message = "New password and confirm must match"
                    });

                    return false;
                }

                var result = await WebRequests.PostAsync<ApiResponse<UpdateUserPasswordApiModel>>(
                    RouteHelpers.GetAbsoluteUrl(ApiRoutes.UpdateUserPassword),
                    new UpdateUserPasswordApiModel
                    {
                        CurrentPassword = Password.CurrentPassword.Unsecure(),
                        NewPassword = Password.NewPassword.Unsecure()
                    },
                    bearerToken: credentials.Token);

                // The resonse has an error...
                if (await result.DisplayErrorIfFailedAsync("Change Password"))
                {
                    // Log it
                    Logger.LogDebugSource($"Failed to change password. {result.ErrorMessage}");

                    // Return false
                    return false;
                }

                // Log it
                Logger.LogDebugSource("Successfully changed password.");
                PasswordChangedDate = DateTimeOffset.UtcNow.ToString("dd/mm/yy");
                return true;
            });


        }
        public async Task<bool> EditProfilePictureAsync()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.jpe;*.bmp",
                Multiselect = false,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var credentials = await ClientDataStore.GetLoginCredentialsAsync();

                // Uploading picture and updating ProfilePictureName
                var result = await FTPServices.UploadAsync(
                    openFileDialog.FileName,
                    FileType.ProfilePicture,
                    credentials: credentials);

                if (result != null)
                {
                    var request = await WebRequests.PostAsync<ApiResponse<UserProfileDetailsApiModel>>(
                        RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetUserProfile),
                        bearerToken: credentials.Token);

                    if (await request.DisplayErrorIfFailedAsync("Network Problem"))
                        return false;

                    var userProfile = request.ServerResponse.Response;

                    await FTPServices.DownloadThumbAsync(userProfile.ProfilePictureThumbName, FileType.ProfilePicture);

                    ProfilePicName = userProfile.ProfilePictureName;
                    ProfilePicUri = initialPath + userProfile.ProfilePictureName;
                    ProfilePicThumbName = userProfile.ProfilePictureThumbName;
                    ProfilePicThumbUri = initialPath + userProfile.ProfilePictureThumbName;

                    var newCredentials = credentials;
                    newCredentials.ProfilePicName = ProfilePicName;
                    newCredentials.ProfilePicThumbName = ProfilePicThumbName;
                    OnPropertyChanged(nameof(ProfilePicUri));
                    OnPropertyChanged(nameof(ProfilePicThumbUri));

                    await ClientDataStore.SaveLoginCredentialsAsync(newCredentials);
                }
                else
                    return false;
            }
            return true;
        }
        public async Task LogoutAsync()
        {
            ClearUserData();
            AccountInfo.AccountInfoVisible = false;
            ViewModelApplication.ChatPageVisible = false;
            ViewModelApplication.IsLoggedOut = true;
            if (ChatList.SelectedBefore != null)
                ChatList.SelectedBefore.IsSelected = false;

            ViewModelApplication.GoToPage(ApplicationPage.Login);
            await RunCommandAsync(() => LoggingOut, async () =>
            {
                await ClientDataStore.ClearDataStoreAsync();
            });
        }
        public async Task UpdateValuesFromLocalStoreAsync()
        {
            // Get stored credentials
            var getLoginCredentials = await ClientDataStore.GetLoginCredentialsAsync();

            FirstName.OriginalText = getLoginCredentials?.FirstName;
            LastName.OriginalText = getLoginCredentials?.LastName;
            Username.OriginalText = getLoginCredentials?.Username;
            Email.OriginalText = getLoginCredentials?.Email;
            PasswordChangedDate = getLoginCredentials?.PasswordChangedDate;
            LastSignInDate = getLoginCredentials?.LastSignInDate;

            ViewModelSettings.Name = StringHelpers.GetShortName(firstName: FirstName.OriginalText,
                                                                lastName: LastName.OriginalText,
                                                                userName: Username.OriginalText);

            OnPropertyChanged(nameof(ProfilePicThumbUri));
            OnPropertyChanged(nameof(ProfilePicUri));
        }
        public void ClearUserData()
        {
            FirstName.OriginalText = mLoadingText;
            LastName.OriginalText = mLoadingText;
            Username.OriginalText = mLoadingText;
            Email.OriginalText = mLoadingText;

            ProfilePicName = null;
            ProfilePicUri = null;
            ProfilePicThumbName = null;
            ProfilePicThumbUri = null;
            LastSignInDate = null;
            PasswordChangedDate = null;

            ContactList.Items.Clear();
            ChatList.Items.Clear();
            ChatMessageList.ClearChatPage();
            ViewModelApplication.ChatPageVisible = false;
        }

        public async Task<bool> UpdateUserCredentailsValueAsync(
                                   string displayName,
                                   Expression<Func<LoginCredentialsDataModel, string>> propertyToChange,
                                   string newValue,
                                   Action<UpdateUserProfileApiModel, string> setApiModel)
        {
            // Log it
            Logger.LogDebugSource($"Saving {displayName}...");

            // Get the current known credentials
            var credentials = await ClientDataStore.GetLoginCredentialsAsync();

            // Get the property to update from the credentials
            var toUpdate = propertyToChange.GetPropertyValue(credentials);

            // Log it
            Logger.LogDebugSource($"{displayName} currently {toUpdate} updating to {newValue}");

            // Check if the value is the same. If so...
            if (toUpdate == newValue)
            {
                // Log it
                Logger.LogDebugSource($"{displayName} the same, ignoring...");
                return true;
            }

            // Set the property
            propertyToChange.SetPropertyValue(newValue, credentials);

            // Create update deatails
            var updateApiModel = new UpdateUserProfileApiModel();

            // Ask caller to set appropriate value
            setApiModel(updateApiModel, newValue);

            var result = await WebRequests.PostAsync<ApiResponse>(
                RouteHelpers.GetAbsoluteUrl(ApiRoutes.UpdateUserProfile),
                // Pass the api model 
                updateApiModel,
                bearerToken: credentials.Token);

            // The resonse has an error...
            if (await result.DisplayErrorIfFailedAsync($"Update {displayName}"))
            {
                // Log it
                Logger.LogDebugSource($"Failed to update {displayName}. {result.ErrorMessage}");

                // Return false
                return false;
            }

            // Log it
            Logger.LogDebugSource($"Successfully updated {displayName}. Saving to the local cache...");

            // Store the new user credentials the data store
            await ClientDataStore.SaveLoginCredentialsAsync(credentials);

            ViewModelSettings.Name = StringHelpers.GetShortName(firstName: FirstName.OriginalText,
                                                               lastName: LastName.OriginalText,
                                                               userName: Username.OriginalText);
            // Return successful
            return true;
        }
        #endregion
    }
}