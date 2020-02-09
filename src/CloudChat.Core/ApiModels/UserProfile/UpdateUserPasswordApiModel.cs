namespace CloudChat.Core
{
    public class UpdateUserPasswordApiModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
