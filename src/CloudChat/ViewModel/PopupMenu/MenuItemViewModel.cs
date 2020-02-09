using CloudChat.Core;
using System.Windows.Input;

namespace CloudChat
{
    public class MenuItemViewModel : BaseViewModel
    {
        public string Text { get; set; }
        public MenuItemType Type { get; set; }
        public IconType Icon { get; set; }
        public ICommand OptionCommand { get; set; }
    }
}