using System.Windows;
using System.Windows.Controls;


namespace CloudChat
{
    public class DialogWindowViewModel : WindowViewModel
    {
        public string Title { get; set; }
        public Control Content { get; set; }

        public DialogWindowViewModel(Window window) : base(window)
        {
            WindowMinimumHeight = 150;
            WindowMinimumWidth = 350;
            TitleHeight = 25;
            OuterMarginSize = new Thickness(10);
        }

    }
}