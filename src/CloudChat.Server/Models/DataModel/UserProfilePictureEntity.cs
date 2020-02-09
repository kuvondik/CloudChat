namespace CloudChat.Server.Models
{
    public class UserProfilePictureEntity
    {
        public string Id { get; set; }
        public UsersEntity User { get; set; }
        public string PictureUrl { get; set; }
        public string PictureThumbUrl { get; set; }
    }
}
