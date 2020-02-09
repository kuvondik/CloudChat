using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for ContactList.xaml
    /// </summary>
    public partial class ContactListControl : UserControl
    {
        public ContactListControl()
        {
            InitializeComponent();
            DataContext = DI.ContactList;
        }
    }
}
