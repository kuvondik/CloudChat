using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for ChatInfoControl.xaml
    /// </summary>
    public partial class ChatInfoControl : UserControl
    {
        public ChatInfoControl()
        {
            InitializeComponent();
            DataContext = DI.ChatInfo;
        }
    }
}
