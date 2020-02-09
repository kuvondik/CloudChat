using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace CloudChat
{
    public class SettingsDesignModel : SettingsViewModel
    {
        public static SettingsDesignModel Instance = new SettingsDesignModel();
        public SettingsDesignModel()
        {
            Privacy = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/lock.png",
                Title = "Privacy",
            };

            Share = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/heart.png",
                Title = "Tell a friend",
            };

            Help = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/help.png",
                Title = "Help",
            };
            Account = new SettingsOptionViewModel
            {
                IconPath = "/Images/Icons/person.png",
                Title = "Account",
            };
        }
        
    }
}
