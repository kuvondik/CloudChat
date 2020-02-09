namespace CloudChat.Core
{
    public static class ApiRoutes
    {
        public const string Register = "api/register";
        public const string Login = "api/login";
        public const string VerifyEmail = "api/verify/email";
        public const string GetUserProfile = "api/user/profile";
        public const string UpdateUserProfile = "api/user/profile/update";
        public const string UpdateUserPassword = "api/user/password/update";

        public const string AddContact = "chat/add/contact";
        public const string GetContacts = "chat/get/contacts";

        public const string GetUserInfo = "chat/get/user/info";

        public const string AddConversation = "chat/add/conversation";
        public const string GetConversations = "chat/get/conversations";
        public const string GetNewConversations = "chat/check/new/conversations";

        public const string AddMessage = "chat/add/message";
        public const string GetMessages = "chat/get/messages";
        public const string GetNewMessages = "chat/check/new/messages";

        public const string AddGroup = "chat/add/group";
        public const string GetGroups = "chat/get/groups";
        public const string GetNewGroups = "chat/check/new/groups";

    }
}
