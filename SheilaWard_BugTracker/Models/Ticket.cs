using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Models
{
    public class Ticket
    {
        // keys
        public int Id { get; set; }                    // Id number of the ticket

        public int ProjectId { get; set; }             // Foreign Key - Project the ticket belongs to

        [Display(Name = "Type")]
        public int TicketTypeId { get; set; }          // Foreign Key - Type of ticket: Defect, Documentation, Enhancement

        [Display(Name = "Priority")]
        public int? TicketPriorityId { get; set; }     // Foreign Key - Priority of ticket: Low, Medium, High, Urgent

        [Display(Name = "Status")]
        public int TicketStatusId { get; set; }        // Foreign Key - Status of ticket: Inactive, Active/Unassigned, Active/Assigned, Completed, Archived

        [Display(Name = "Owner")]
        public string OwnerUserId { get; set; }        // Foreign Key - Owner of Ticket (the Submitter who entered the ticket into the system?  Or the Project Manager it would be under?)

        [Display(Name = "Assigned To")]
        public string AssignedToUserId { get; set; }  // Foreign Key - User that the Ticket is assigned to - what if this is null?

        // rest of db properties
        [Display(Name = "Title")]
        public string Title { get; set; }              // Title of the ticket

        [Display(Name = "Description")]
        public string Description { get; set; }        // Description of the ticket

        [Display(Name = "Created")]
        public DateTimeOffset Created { get; set; }    // When the ticket was added into the system

        [Display(Name = "Updated")]
        public DateTimeOffset? Updated { get; set; }   // Last time the ticket was updated in the system

        // nav properties - Parent
        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        // nav properties - Children
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

        public Ticket()
        {
            TicketComments = new HashSet<TicketComment>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifications = new HashSet<TicketNotification>();
        }
    }
}