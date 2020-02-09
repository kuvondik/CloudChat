using System.Windows.Controls;
using System.Windows.Input;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        public SideMenuControl()
        {
            InitializeComponent();
        }

        private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta == 0.0)
                return;
            if (e.Delta > 0.0)
            {
                DI.ViewModelApplication.SearchVisible = false;
                (Template.FindName("searchTextBox", this) as SearchTextBox).Text = "";
            }
            else
                DI.ViewModelApplication.SearchVisible = true;
        }
    }
}