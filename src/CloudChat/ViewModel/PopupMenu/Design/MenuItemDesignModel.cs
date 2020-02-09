using CloudChat.Core;

namespace CloudChat
{
    public class MenuItemDesignModel : MenuItemViewModel
    {
        public static MenuItemDesignModel Instance = new MenuItemDesignModel();

        public MenuItemDesignModel()
        {
            Text = "Hello World!";
            Icon = IconType.FileManager;
        }
    }
}