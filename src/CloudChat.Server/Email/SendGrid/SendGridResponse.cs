using System.Collections.Generic;

namespace CloudChat.Server
{
    public class SendGridResponse
    {
        public List<SendGridResponseError> Errors { get; set; }
    }
}
