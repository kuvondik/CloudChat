namespace CloudChat
{
    public class ChatListItemDesignModel : ChatListItemViewModel
    {
        public static ChatListItemDesignModel Instance => new ChatListItemDesignModel();

        public ChatListItemDesignModel()
        {
            Initials = "QS";
            Title = "Quvondiq";
            Message = "This new chat app is awesome. I bet it will be fast too!";
            ProfilePictureRGB = "EB4133";
            NewContentAvailable = true;
            MessageDate = "10:10";
        }
    }
}