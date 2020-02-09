using System.ComponentModel;
using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for ProfileInfoControl.xaml
    /// </summary>
    public partial class AccountInfoControl : UserControl
    {
        public AccountInfoControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                DataContext = new AccountInfoViewModel();
            else
                DataContext = DI.AccountInfo;
        }
    }
}
