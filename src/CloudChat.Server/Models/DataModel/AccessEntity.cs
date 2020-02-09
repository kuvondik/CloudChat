using System;

namespace CloudChat.Server.Models
{
    public class AccessEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
        public string DeviceId { get; set; }
    }
}
