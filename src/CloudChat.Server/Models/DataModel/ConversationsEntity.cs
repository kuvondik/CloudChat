using CloudChat.Core;
using System;

namespace CloudChat.Server.Models
{
    public class ConversationsEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ChannelId { get; set; }
        public ConversationType Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public UsersEntity Creator { get; set; }
    }
}
