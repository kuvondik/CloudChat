using System;
using System.Collections.Generic;

namespace CloudChat.Core
{
    public class ConversationApiModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> UsernameList { get; set; }
        public string ChannelId { get; set; }
        public string PreMessage { get; set; }
        public ConversationType Type{ get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string CreatorUsername { get; set; }

        public ConversationApiModel()
        {
            UsernameList = new List<string>();
        }

        public ConversationApiModel DeepCopy()
        {
            var other = (ConversationApiModel)MemberwiseClone();
            other.Id = string.Copy(Id);
            other.Title = Title == null? null:string.Copy(Title);
            other.ChannelId = string.Copy(ChannelId);
            other.PreMessage = (PreMessage == null) ? null : string.Copy(PreMessage);
            other.Type= Type;
            other.CreatedAt = CreatedAt;
            other.UpdatedAt = UpdatedAt;
            other.CreatorUsername = string.Copy(CreatorUsername);
            other.UsernameList =new List<string>(UsernameList.ToArray());

            return other;
        }

        public void AddParticipantsUsername(IEnumerable<ParticipantApiModel> participantApiModels)
        {
            foreach (var participant in participantApiModels)
            {
                if (participant.UserUsername != CreatorUsername)
                    UsernameList.Add(participant.UserUsername);
            }
        }
    }
}
