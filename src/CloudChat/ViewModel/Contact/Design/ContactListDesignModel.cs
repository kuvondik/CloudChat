using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudChat
{
    public class ContactListDesignModel:ContactListViewModel
    {
        public static ContactListDesignModel Instance => new ContactListDesignModel();

        public ContactListDesignModel()
        {
            Items = new ObservableCollection<ContactItemViewModel>
            {
                new ContactItemViewModel
                {
                    FirstName = "Jack",
                    LastName = "Brown",
                    Initials = "JB",
                    Rgb = new RgbValue().GetRandomRgbValue(),
                    Email = "jack@website.com",
                    PhoneNumber = "998971234567",
                },

                new ContactItemViewModel
                {
                    FirstName = "Kuvondik",
                    LastName = "Sayfiddinov",
                    Initials = "KS",
                    Rgb = new RgbValue().GetRandomRgbValue(),
                    Email = "kuvondik@website.com",
                    PhoneNumber = "998971234567",
                },

                new ContactItemViewModel
                {
                    FirstName = "Jack",
                    LastName = "Brown",
                    Initials = "JB",
                    Rgb = new RgbValue().GetRandomRgbValue(),
                    Email = "jack@website.com",
                    PhoneNumber = "998971234567",
                },

                new ContactItemViewModel
                {
                    FirstName = "Jack",
                    LastName = "Brown",
                    Rgb = new RgbValue().GetRandomRgbValue(),
                    Initials = "JB",
                    Email = "jack@website.com",
                    PhoneNumber = "998971234567",

                },
            };
        }
    }
}
