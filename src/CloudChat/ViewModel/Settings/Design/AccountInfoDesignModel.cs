    using System.Collections.Generic;

namespace CloudChat
{
    public class AccountInfoDesignModel : AccountInfoViewModel
    {
        public static AccountInfoDesignModel Instance => new AccountInfoDesignModel();

        public AccountInfoDesignModel()
        {
            FirstName = new TextEntryViewModel { Label = "FirstName", OriginalText = "Kuvondik",Working=false,Editing=true };
            LastName = new TextEntryViewModel { Label="LastName", OriginalText="Sayfiddinov", Working = false, Editing = true };
            Username = new TextEntryViewModel { Label="Username", OriginalText="C5h4rp3r", Working = false, Editing = true };
            Password = new PasswordEntryViewModel { Label="Password", FakePassword="********", Working = false, Editing = true };
            Email = new TextEntryViewModel { Label="Email", OriginalText="quvondiqbek007@gmail.com", Working = false, Editing = true };
        }
    }
}