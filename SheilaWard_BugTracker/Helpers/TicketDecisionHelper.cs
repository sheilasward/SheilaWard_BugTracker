using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Enumerations;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public class TicketDecisionHelper : CommonHelper
    {
        //public bool TicketDetailIsViewableByUser(int ticketId)
        //{
        //    var userId = HttpContext.Current.User.Identity.GetUserId();
        //    var roleName = roleHelper.ListUserRoles(userId).FirstOrDefault();
        //    var systemRole = (SystemRole)Enum.Parse(typeof(SystemRole), roleName);
        //}

        public bool TicketIsEditableByUser(Ticket ticket)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch(myRole)
            {
                case "Developer":
                    return ticket.AssignedToUserId == userId;   // Ticket is assigned to this developer - returns true
                case "Submitter":
                    return ticket.OwnerUserId == userId;        // This submitter is the owner of this ticket - returns true
                case "ProjectManager":
                    // Look for the userId Projects - then get all its Tickets, then just select the Ids.  See if this list contains this ticket's Id.
                    return db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case "Admin":
                    return true;
                default:
                    return false;
            }
        }
        
        public ICollection<Ticket> ListOfUsersTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Developer":
                    return db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                case "Submitter":
                    return db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                case "ProjectManager":
                    return db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
                default:
                    return db.Tickets.ToList();
            }
                
        }

        public int GetTicketCount(string status)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
             if (status == "All")
            {
                switch (myRole)
                {
                    case "Developer":
                        return db.Tickets.Where(t => t.AssignedToUserId == userId).Count();
                    case "Submitter":
                        return db.Tickets.Where(t => t.OwnerUserId == userId).Count();
                    case "ProjectManager":
                        return db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).Count();
                    default:
                        return db.Tickets.Count();
                }
            }
            else
            {
                switch (myRole)
                {
                    case "Developer":
                        return db.Tickets.Where(t => t.TicketStatus.Name == status && t.AssignedToUserId == userId).Count();
                    case "Submitter":
                        return db.Tickets.Where(t => t.TicketStatus.Name == status && t.OwnerUserId == userId).Count();
                    case "ProjectManager":
                        var pmTickets = db.Users.Find(userId).Projects.SelectMany(t => t.Tickets);
                        return pmTickets.Where(t => t.TicketStatus.Name == status).Count();
                    default:
                        return db.Tickets.Where(t => t.TicketStatus.Name == status).Count();
                }
            }
            
        }

        public bool DevIsOnProject(Ticket ticket)
        {
            return currentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
        }

    }
}