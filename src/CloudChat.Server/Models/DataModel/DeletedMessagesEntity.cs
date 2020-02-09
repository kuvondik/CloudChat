using System;

namespace CloudChat.Server.Models
{
    public class DeletedMessagesEntity
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
