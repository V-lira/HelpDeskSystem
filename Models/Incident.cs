using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDeskSystem.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string AssignedUser { get; set; }
        public string Department { get; set; }
        public string SLA { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public virtual ICollection<IncidentComment> Comments
        {
            get;
            set;
        }
    }
}