namespace CloudChat   
{
    public class PasswordEntryDesignModel:PasswordEntryViewModel
    {
        public static PasswordEntryDesignModel Instance = new PasswordEntryDesignModel();
        public PasswordEntryDesignModel()
        {
            Label = "Password";
            FakePassword = "********";
            Editing = false;
            Working = false;
        }
    }
}
