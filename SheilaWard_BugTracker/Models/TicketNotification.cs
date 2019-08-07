using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Models
{
    public class TicketNotification
    {
        // keys
        public int Id { get; set; }                 // Primary Key
        public int TicketId { get; set; }           // Foreign Key
        public string RecipientId { get; set; }     // Foreign Key
        public string SenderId { get; set; }        // Foreign Key
    
        // other properties
        public DateTimeOffset Created { get; set; }
        public string Subject { get; set; }
        public string NotificationBody { get; set; }
        public bool IsRead { get; set; }



        //nav
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
        public virtual ApplicationUser Sender { get; set; }
    }
}