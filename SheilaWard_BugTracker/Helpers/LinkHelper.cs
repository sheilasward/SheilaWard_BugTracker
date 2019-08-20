using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public class LinkHelper : CommonHelper
    {
        public List<string> UserCanEditTicket(Ticket ticket)
        {
            var list = new List<string>(); 
            switch (this.currentRole)
            {
                case "Admin":
                    list.Add("User, Type, Title, Description, Priority, Status, Comment, Attachment");
                    return list;
                case "ProjectManager":
                    if (currentUser.Projects.SelectMany(t => t.Tickets).Select(t => t.Id).Contains(ticket.Id))
                    {
                        list.Add("User, Type, Title, Description, Priority, Status, Comment, Attachment");
                    }
                    return list;
                case "Developer":
                    if (currentUser.Id == ticket.AssignedToUserId)
                    {
                        list.Add("Type, Title, Description, Priority, Comment, Attachment");
                    }
                    return list;
                case "Submitter":
                    if (currentUser.Id == ticket.OwnerUserId)
                    {
                        list.Add("Type, Title, Description, Priority, Comment, Attachment");
                    }
                    return list;
                default: return list;
            }
        }
    }
}