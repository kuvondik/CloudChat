using System;

namespace CloudChat.Core
{
    public class UpdateUserProfileApiModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePictureName { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
