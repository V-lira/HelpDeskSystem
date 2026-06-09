using System;

namespace HelpDeskSystem.Models
{
    public class IncidentComment
    {
        public int Id { get; set; }
        public int IncidentId { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}