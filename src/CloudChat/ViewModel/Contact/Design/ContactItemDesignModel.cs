using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudChat
{
    public class ContactItemDesignModel:ContactItemViewModel
    {
        public static ContactItemDesignModel Instance => new ContactItemDesignModel();

        public ContactItemDesignModel()
        {
            FirstName = "Jack";
            LastName = "Brown";
            Initials = "JB";
            Rgb = new RgbValue().GetRandomRgbValue();
            Email = "jack@website.com";
            PhoneNumber = "998971234567";
        }
    }
}
