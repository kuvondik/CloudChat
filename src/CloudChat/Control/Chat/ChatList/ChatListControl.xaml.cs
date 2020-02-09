using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for ChatListControl.xaml
    /// </summary>
    public partial class ChatListControl : UserControl
    {
        public ChatListControl()
        {
            InitializeComponent();
            DataContext = DI.ChatList;
        }
    }
}