using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Models
{
    public class TicketComment
    {
        // keys
        public int Id { get; set; }                  // Primary Key
        public int TicketId { get; set; }            // Foreign Key - What Ticket does this Comment belong to?
        public string AuthorId { get; set; }         // Foreign Key - What User made this comment?

        // other properties
        public string Comment { get; set; }           // Comment
        public DateTimeOffset Created { get; set; }   // When Comment was entered into the system

        // nav properties
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}