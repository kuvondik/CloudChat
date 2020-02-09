using CloudChat.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudChat.Server
{
    public class ParticipantDetails
    {
        public ParticipantsEntity Participant { get; set; }
        public ConversationsEntity Conversation { get; set; }
        public UsersEntity Creator { get; set; }
    }
}
