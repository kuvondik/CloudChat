using Dna;
using CloudChat.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CloudChat
{
    public class ContactListViewModel : BaseViewModel
    {
        #region Private Members

        private ObservableCollection<ContactItemViewModel> mItems;
        #endregion

        #region Public Properties
        public ObservableCollection<ContactItemViewModel> Items
        {
            get => mItems;
            set
            {
                if (mItems != value)
                    mItems = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public ContactItemViewModel SelectedBefore { get; set; }
        public bool ContactsAreDownloading { get; set; }
        #endregion

        #region Public Commands

        public ICommand OpenAddContactFormCommand { get; set; }
        #endregion

        #region Constructor

        public ContactListViewModel()
        {
            Items = new ObservableCollection<ContactItemViewModel>();
            OpenAddContactFormCommand = new RelayCommand(OpenAddContactForm);
        }
        #endregion

        #region Command Methods

        public void OpenAddContactForm()
        {
            DI.ViewModelApplication.AddContactFormVisible = true;
            DI.ViewModelApplication.DimmableOverlayVisible = true;
        }
        #endregion

        public void DownloadContacts()
        {
            var result = default(bool);
            RunCommandAsync(() => ContactsAreDownloading, async () =>
              {
                  var credentials = await DI.ClientDataStore.GetLoginCredentialsAsync();

                  var request = await WebRequests.PostAsync<ApiResponse<IEnumerable<ContactApiModel>>>(
                      RouteHelpers.GetAbsoluteUrl(ApiRoutes.GetContacts),
                      credentials?.Username,
                      bearerToken: credentials?.Token);

                  if (request.Successful)
                  {
                      var contactList = request.ServerResponse?.Response;
                      Application.Current.Dispatcher.Invoke(() =>
                      {
                          contactList?.ToList()
                                     .ForEach(x =>
                                     {
                                         Items.Add(new ContactItemViewModel
                                         {
                                             Username = x.Username,
                                             FirstName = x.FirstName,
                                             LastName = x.LastName,
                                             Initials =StringHelpers.GetInitials(x.Username),
                                             Email = x.Email,
                                             PhoneNumber = x.PhoneNumber,
                                             PictureThumbName = x.PictureThumbName,
                                             Rgb = new RgbValue().GetRandomRgbValue(),
                                             PictureUrl = x.PictureName,
                                         });
                                     });
                          result = true;
                      });
                  }
              })
            .ContinueWith(t =>
            {
                if (result)
                    // If Contacts are uploaded ...
                    DI.ViewModelApplication.ContactsAreDownloaded = true;
            });
        }
    }
}
