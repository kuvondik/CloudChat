namespace CloudChat
{
    public class SettingsOptionDesignModel:SettingsOptionViewModel
    {
        public static SettingsOptionDesignModel Instance = new SettingsOptionDesignModel();

        public SettingsOptionDesignModel()
        {
            IconPath = "/Images/Icons/heart.png";
            Title = "Tell a friend";
        }
    }
}
