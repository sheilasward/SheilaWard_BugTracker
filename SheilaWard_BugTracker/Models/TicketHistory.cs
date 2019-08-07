using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Models
{
    public class TicketHistory
    {
        // keys
        public int Id { get; set; }                       // Primary key
        public int TicketId { get; set; }                 // Foreign key
        public string UserId { get; set; }                // Foreign Key - Who changed the ticket?

        // other properties
        public string Property { get; set; }               // Name of property that change
        public string OldValue { get; set; }               // Value before change
        public string NewValue { get; set; }               // Value after change
        public DateTimeOffset DateChanged { get; set; }    // When changed

        // nav properties
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}