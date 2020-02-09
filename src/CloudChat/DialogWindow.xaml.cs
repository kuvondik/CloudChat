using System.Windows;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private DialogWindowViewModel mViewModel;

        public DialogWindowViewModel ViewModel
        {
            get => mViewModel;
            set
            {
                if (mViewModel != value)
                {
                    mViewModel = value;
                    DataContext = mViewModel;
                }
            }
        }
        public DialogWindow()
        {
            InitializeComponent();
        }
    }
}
