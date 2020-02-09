using System;

namespace CloudChat.Core
{
    public class UserProfileDetailsApiModel
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureThumbName { get; set; }
        public string ProfilePictureName { get; set; }

        public LoginCredentialsDataModel ToLoginCredentialsDataModel()
        {
            return new LoginCredentialsDataModel
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Token = Token,
                ProfilePicName = ProfilePictureName,
                ProfilePicThumbName = ProfilePictureThumbName,
                LastSignInDate = DateTimeOffset.UtcNow.ToLocalTime().ToString("dd/MM/yyyy h:mm tt"),
            };
        }
    }
}
