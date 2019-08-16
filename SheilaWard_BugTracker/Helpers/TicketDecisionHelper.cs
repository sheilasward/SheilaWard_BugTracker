using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public class TicketDecisionHelper : CommonHelper
    {

        //public static bool TicketDetailIsViewableByUser(int ticketId)
        //{
        //    var userId = HttpContext.Current.User.Identity.GetUserId();
        //    var roleName = roleHelper.ListUserRoles(userId).FirstOrDefault();
        //    var systemRole = (SystemRole)Enum.Parse(typeof(SystemRole), roleName);
        //}

        public static bool TicketIsEditableByUser(Ticket ticket)
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

    }
}