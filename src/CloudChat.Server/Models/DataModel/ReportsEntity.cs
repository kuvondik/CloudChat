using System;

namespace CloudChat.Server.Models
{
    public class ReportsEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ParticipantId { get; set; }
        public string ReportType { get; set; }
        public string Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
