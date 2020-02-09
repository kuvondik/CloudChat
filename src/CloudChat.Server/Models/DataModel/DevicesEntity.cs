namespace CloudChat.Server.Models
{
    public class DevicesEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public string Type { get; set; }
        public string DeviceToken { get; set; }
    }
}
