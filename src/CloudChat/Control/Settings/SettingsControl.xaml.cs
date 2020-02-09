using System.ComponentModel;
using System.Windows.Controls;


namespace CloudChat
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                DataContext = new SettingsViewModel();
            else
                DataContext = DI.ViewModelSettings;
        }
    }
}
