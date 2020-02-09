using CloudChat.Core;
using System.Collections.Generic;

namespace CloudChat
{
    public class MenuDesignModel:MenuViewModel
    {
        public static MenuDesignModel Instance = new MenuDesignModel();
        public MenuDesignModel()
        {
            Items = new List<MenuItemViewModel>
            {
                new MenuItemViewModel{Type=MenuItemType.Header, Text="Design time header..."},
                new MenuItemViewModel{Text="Menu Item 1", Icon=IconType.FileManager},
                new MenuItemViewModel{Text="Menu iTem 2", Icon=IconType.Picture},
            };
        }
    }
}
