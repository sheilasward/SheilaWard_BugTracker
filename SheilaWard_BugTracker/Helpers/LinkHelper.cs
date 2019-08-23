using SheilaWard_BugTracker.Enumerations;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public class LinkHelper : CommonHelper
    {
        public bool UserCanEditTicketStatus(Ticket ticket)
        {
            switch (currentRole)
            {
                case SystemRole.Admin:
                    return true;
                case SystemRole.ProjectManager:
                    return currentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case SystemRole.Developer:
                case SystemRole.Submitter:
                default:
                    return false;
            }
        }

        public bool UserCanEditTicketPriorityTypeTitleDescription(Ticket ticket)
        {
            switch (currentRole)
            {
                case SystemRole.Admin:
                    return true;
                case SystemRole.ProjectManager:
                    return currentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case SystemRole.Developer:
                    return ticket.AssignedToUserId == currentUser.Id;
                case SystemRole.Submitter:
                    return ticket.OwnerUserId == currentUser.Id;
                default:
                    return false;
            }
        }

        public bool UserCanEditTicketAssignment(Ticket ticket)
        {
            switch (currentRole)
            {
                case SystemRole.Admin:
                    return true;
                case SystemRole.ProjectManager:
                    return currentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id);
                case SystemRole.Developer:
                case SystemRole.Submitter:
                default:
                    return false;
            }
        }
    }
}