using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for SettingsOptionControl.xaml
    /// </summary>
    public partial class SettingsOptionControl : UserControl
    {
        public SettingsOptionControl()
        {
            InitializeComponent();
            DataContext = new SettingsOptionViewModel();
        }
    }
}
