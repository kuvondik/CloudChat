using System;
using System.Collections.ObjectModel;

namespace CloudChat
{
    public class ChatMessageListDesignModel : ChatMessageListViewModel
    {
        public static ChatMessageListDesignModel Instance => new ChatMessageListDesignModel();


        public ChatMessageListDesignModel()
        {
            Title = "Jamol";
            Items = new ObservableCollection<ChatMessageItemViewModel>
            {
               new ChatMessageItemViewModel
               {
                   SenderUsername="Jamol",
                   Message="Hello Quvondiq!",
                   ProfilePictureRGB="EB4133",
                   IsSentByMe=false,
                   Initials="JQ",
                   MessageReadTime=DateTimeOffset.UtcNow.ToString(),
               },
               new ChatMessageItemViewModel
               {
                   SenderUsername="Quvondiq",
                   Message="Hi!, This chat app is awesome! When is it come up?",
                   ProfilePictureRGB="3499c5",
                   IsSentByMe=true,
                   Initials="QS",
                   MessageReadTime=DateTimeOffset.UtcNow.ToString(),
                   MessageSentTime=DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(1)).ToString()
               },
               new ChatMessageItemViewModel
               {
                   SenderUsername="Jamol",
                   Message="I don't know. How are you?",
                   ProfilePictureRGB="EB4133",
                   IsSentByMe=false,
                   Initials="JQ",   
                   MessageReadTime=DateTimeOffset.UtcNow.ToString()
               },
            };
        }
    }
}