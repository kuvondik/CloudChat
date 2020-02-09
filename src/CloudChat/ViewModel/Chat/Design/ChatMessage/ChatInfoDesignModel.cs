using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudChat
{
    public class ChatInfoDesignModel:ChatInfoControlViewModel
    {
        public static ChatInfoDesignModel Instance => new ChatInfoDesignModel();

        public ChatInfoDesignModel()
        {
            FirstName = "Kuvondik";
            LastName = "Sayfiddinov";
            Email = "kuvondik@outlook.com";
            PhoneNumber = "+998977636467";
            PictureThumbnailUrl = "\\Images\\Sample\\Labrador.jpg";
        }
    }
}
