using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for SearchTextBoxControl.xaml
    /// </summary>
    public partial class SearchTextBox : TextBox
    {
        public SearchTextBox()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Text = "";
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            Focus();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Text = "";
        }
    }
}
