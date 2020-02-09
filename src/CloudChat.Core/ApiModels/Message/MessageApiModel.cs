using System;

namespace CloudChat.Core
{
    public class MessageApiModel
    {
        public string ConversationChannelId { get; set; }
        public string EmojiName { get; set; }
        public string SenderUsername { get; set; }
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
        public string AttachmentThumbUrl { get; set; }
        public string AttachmentUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ReadAt { get; set; }

        public MessageApiModel DeepCopy()
        {
            var other = (MessageApiModel)MemberwiseClone();
            other.ConversationChannelId = string.Copy(ConversationChannelId);
            other.SenderUsername = string.Copy(SenderUsername);
            other.Message = (Message == null) ? null : string.Copy(Message);
            other.CreatedAt = CreatedAt;
            other.ReadAt = ReadAt;
            other.MessageType = MessageType;
            other.AttachmentThumbUrl = (AttachmentThumbUrl == null) ? null : string.Copy(AttachmentThumbUrl);
            other.AttachmentUrl = (AttachmentUrl == null) ? null : string.Copy(AttachmentUrl);

            return other;
        }
    }
}
