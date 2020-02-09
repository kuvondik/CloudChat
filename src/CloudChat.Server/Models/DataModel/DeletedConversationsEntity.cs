using System;

namespace CloudChat.Server.Models
{
    public class DeletedConversationsEntity
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
