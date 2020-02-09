using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudChat
{
    public class PasswordEntryViewModel : BaseViewModel
    {
        #region Public properties

        public string Label { get; set; }
        public string FakePassword { get; set; }

        public SecureString CurrentPassword { get; set; }
        public SecureString NewPassword { get; set; }
        public SecureString ConfirmPassword { get; set; }

        public string CurrentPasswordHintText { set; get; }
        public string NewPasswordHintText { get; set; }
        public string ConfirmPasswordHintText { get; set; }
        public bool Editing { get; set; }
        public bool Working { get; set; }
        public Func<Task<bool>> CommitAction { get; set; }

        #endregion

        #region Public commmands

        public ICommand EditCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Constructor

        public PasswordEntryViewModel()
        {
            CurrentPasswordHintText = "Current Password";
            NewPasswordHintText = "New Password";
            ConfirmPasswordHintText = "Confirm Password";

            EditCommand = new RelayCommand(Edit);
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(Save);
        }

        #endregion

        #region Command Methods

        public void Save()
        {
            var result = default(bool);
            RunCommandAsync(() => Working, async () =>
            {
                Editing = false;
                result = CommitAction == null ? true : await CommitAction();
            })
            .ContinueWith(t =>
            {
                if (!result)
                // Go back into edit mode
                {
                    Editing = true;
                }
            });
        }

        public void Cancel()
        {
            Editing = false;
        }

        public void Edit()
        {
            Editing = true;
        }
        #endregion
    }
}
