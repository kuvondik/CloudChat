using CloudChat.Core;
using System;

namespace CloudChat.Server.Models
{
    public class MessagesEntity
    {
        public string Id { get; set; }
        public MessageType MessageType { get; set; }
        public string Message { get; set; }

        public string AttachmentThumbUrl { get; set; }
        public string AttachmentUrl { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ReadAt { get; set; }
        public string Guid { get; set; }

        public UsersEntity Sender { get; set; }
        public ConversationsEntity Conversation { get; set; }
        public ParticipantsEntity Participant { get; set; }
    }
}
