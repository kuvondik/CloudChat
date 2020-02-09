using CloudChat.Core;

namespace CloudChat.Server.Models
{
    public class ParticipantsEntity
    {
        public string Id { get; set; }
        public UsersEntity User { get; set; }
        public ConversationsEntity Conversation { get; set; }
    }
}
