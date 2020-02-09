namespace CloudChat
{
    public class ChatListDesignModel : ChatListViewModel
    {
        public static ChatListDesignModel Instance => new ChatListDesignModel();

        public ChatListDesignModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<ChatListItemViewModel>()
            {
                new ChatListItemViewModel
                {
                    Initials = "J",
                    Title = "Jack",
                    Message = "",
                    ProfilePictureRGB = "0F8383",
                    NewContentAvailable = false
                },
                new ChatListItemViewModel
                {
                    Initials = "K",
                    Title = "Kuvondik",
                    Message = "",
                    ProfilePictureRGB = "EB4133",
                },
            };
        }
    }
}