using CloudChat.Core;
using System.IO;
using System.Windows.Input;

namespace CloudChat
{
    public class ContactItemViewModel : BaseViewModel
    {
        protected string mPictureThumbName;

        public string Username { get; set; }
        public string Name => StringHelpers.GetShortName(firstName: FirstName,
                                                          lastName: LastName,
                                                          userName: Username);
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Initials { get; set; }
        public string Rgb { get; set; }
        public string PictureThumbName
        {
            get => mPictureThumbName;
            set
            {
                if (mPictureThumbName == value || value=="")
                    return;
                mPictureThumbName = value;

                if (value == null)
                    return;

                var localPath = Directory.GetCurrentDirectory() + "\\Cache\\" + mPictureThumbName;

                CoreDI.TaskManager.Run(async () =>
                {
                    if (!File.Exists(localPath))
                        await FTPServices.DownloadThumbAsync(mPictureThumbName, FileType.ProfilePicture);
                    PictureThumbnailUrl = localPath;
                });

                OnPropertyChanged(nameof(PictureThumbnailUrl));
            }
        }
        public string PictureThumbnailUrl { get; set; }
        public string PictureUrl { get; set; }
        public bool IsSelected { get; set; }

        public ICommand ShowInfoCommand { get; set; }
        public ICommand DeleteContactCommand { get; set; }
        public ICommand EditContactCommand { get; set; }

        public ContactItemViewModel()
        {
            ShowInfoCommand = new RelayParameterizedCommand(ShowInfo);
            DeleteContactCommand = new RelayCommand(DeleteContact);
            EditContactCommand = new RelayCommand(EditContact);
        }

        public void EditContact()
        {
            CoreDI.TaskManager.Run(async () =>
            {
                await DI.UI.ShowMessage(new MessageBoxDialogViewModel
                {
                    Title = "Sorry",
                    Message = "No Acion, sorry",
                });
            });
        }

        public void DeleteContact()
        {
            // Delete from UI
            DI.ContactList.Items.Remove(this);

            // TODO: Delete from the server
        }

        public void ShowInfo(object parameter)
        {
            if (!IsSelected)
            {
                DI.ChatInfo.Clear();
                if (DI.ContactList.SelectedBefore != null)
                    DI.ContactList.SelectedBefore.IsSelected = false;

                DI.ContactList.SelectedBefore = this;
                IsSelected = true;

                DI.ChatInfo.Username = Username;
                DI.ChatInfo.FirstName = FirstName;
                DI.ChatInfo.LastName = LastName;
                DI.ChatInfo.Email = Email;
                DI.ChatInfo.PhoneNumber = PhoneNumber;
                // TODO: Change it for blocking system
                DI.ChatInfo.SendMessageAvailable = true;
                DI.ChatInfo.PictureThumbnailName = PictureThumbName;
            }

            DI.ViewModelApplication.DimmableOverlayVisible = true;
            DI.ViewModelApplication.ChatInfoControlVisible = true;
        }
    }
}
