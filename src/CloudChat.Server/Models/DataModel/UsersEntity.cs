using System;

namespace CloudChat.Server.Models
{
    public class UsersEntity
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int VerficationCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsReported { get; set; }
        public bool IsBlocked { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset PasswordChangedDate { get; set; }
    }
}
