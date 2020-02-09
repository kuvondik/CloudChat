using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for AddConatactForm.xaml
    /// </summary>
    public partial class AddConatactForm : UserControl
    {
        public AddConatactForm()
        {
            InitializeComponent();
            DataContext = new AddContactFormViewModel();
        }
    }
}
