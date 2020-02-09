using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CloudChat

{
    public class BaseDialogUserControl : UserControl
    {
        private DialogWindow mDialogWindow;
        public int MinimumWidth { get; set; } = 250;
        public int MinimumHeight { get; set; } = 100;
        public int TitleHeight { get; set; } = 25;
        public string Title { get; set; }
        public ICommand CloseCommand { get; set; }

        public BaseDialogUserControl()
        {
            mDialogWindow = new DialogWindow();
            mDialogWindow.ViewModel = new DialogWindowViewModel(mDialogWindow);
            CloseCommand = new RelayCommand(() => mDialogWindow.Close());
        }
        public Task ShowDialog<T>(T viewModel) where T : BaseDialogViewModel
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    mDialogWindow.ViewModel.WindowMinimumHeight = MinimumHeight;
                    mDialogWindow.ViewModel.WindowMinimumWidth = MinimumWidth;
                    mDialogWindow.ViewModel.Title = viewModel.Title ?? Title;
                    mDialogWindow.ViewModel.TitleHeight = TitleHeight;
                    mDialogWindow.ViewModel.Content = this;

                    mDialogWindow.Owner = Application.Current.MainWindow;
                    mDialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    DataContext = viewModel;

                    mDialogWindow.ShowDialog();
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            });


            return tcs.Task;
        }
    }
}
