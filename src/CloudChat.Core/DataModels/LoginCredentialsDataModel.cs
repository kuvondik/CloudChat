namespace CloudChat.Core
{
    public class LoginCredentialsDataModel
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string ProfilePicName { get; set; }

        public string ProfilePicThumbName { get; set; }

        public string PasswordChangedDate { get; set; }

        public string LastSignInDate { get; set; }
    }
}
