using System;

namespace CloudChat.Server.Models
{
    public class BlockListEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ParticipantsId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
