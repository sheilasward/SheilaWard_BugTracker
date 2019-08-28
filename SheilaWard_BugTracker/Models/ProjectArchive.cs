using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Models
{
    public class ProjectArchive
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset Archived { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

        public ProjectArchive()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}