using System.Collections.Generic;

namespace CloudChat.Core
{
    public class SendEmailResponse
    {
        public bool Successful => !(Errors?.Count > 0);
        public List<string> Errors { get; set; }
    }
}
