using System;

namespace CloudChat
{
    public class ChatMessageItemDesignModel : ChatMessageItemViewModel
    {
        public static ChatMessageItemDesignModel Instance => new ChatMessageItemDesignModel();

        public ChatMessageItemDesignModel()
        {
            Message = "Hello World!";
            ProfilePictureRGB = "EB4133";
            IsSentByMe = false;
            SenderUsername = "quvondiq";
            Initials = "q";
            MessageSentTime = "12:12 PM";
        }
    }
}