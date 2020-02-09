using System.Collections.Generic;

namespace CloudChat.Core
{
    public class AddConversationRequestApiModel
    {
        public List<ParticipantApiModel> Participants { get; set; }
        public ConversationApiModel Conversation { get; set; }
    }
}
