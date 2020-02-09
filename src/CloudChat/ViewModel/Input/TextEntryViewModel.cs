using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudChat
{
    public class TextEntryViewModel : BaseViewModel
    {
        #region Public properties

        public string Label { get; set; }
        public string OriginalText { get; set; }
        public string EditedText { get; set; }
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

        public TextEntryViewModel()
        {
            EditCommand = new RelayCommand(Edit);
            CancelCommand = new RelayCommand(Cancel);
            SaveCommand = new RelayCommand(Save);
        }
        #endregion

        #region Command Methods
        public void Save()
        {
            var result = default(bool);
            var oldOriginalText = OriginalText;
            RunCommandAsync(() => Working, async () =>
            {
                Editing = false;
                OriginalText = EditedText;
                result = CommitAction == null ? true : await CommitAction();
            }).ContinueWith(t =>
            {
                if (!result)
                // Go back into edit mode
                {
                    OriginalText = oldOriginalText;
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
            EditedText = OriginalText;
            Editing = true;
        }
        #endregion
    }
}
