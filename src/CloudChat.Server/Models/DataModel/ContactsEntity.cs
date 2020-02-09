using System.ComponentModel.DataAnnotations;

namespace CloudChat.Server.Models
{
    public class ContactsEntity
    {
        public string Id { get; set; }
        public UsersEntity User { get; set; }
        public UsersEntity Owner { get; set; }
    }
}
