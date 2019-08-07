using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Models
{
    public class TicketAttachment
    {
        // keys
        public int Id { get; set; }             // Primary Key
        public int TicketId { get; set; }       // Foreign Key - What Ticket does this Attachment belong to?
        public string UserId { get; set; }      // Foreign Key - What User added this Attachment?
        
        // other properties
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public string AttachmentUrl { get; set; }

        // nav properties
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}