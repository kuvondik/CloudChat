namespace CloudChat.Server.Models
{
    public class ConversationPictureEntity
    {
        public string Id { get; set; }
        public string PictureUri { get; set; }
        public string PictureThumbUri { get; set; }
        public ConversationsEntity Conversation { get; set; }
    }
}
