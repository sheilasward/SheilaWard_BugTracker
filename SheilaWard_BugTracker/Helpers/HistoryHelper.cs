using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SheilaWard_BugTracker.Helpers
{
    public class HistoryHelper : CommonHelper 
    {
        public void RecordHistory(Ticket oldTicket, Ticket newTicket)
        {
            // The only properties I'm interested in are: Title, Description, TicketPriorityId, TicketStatusId, TicketTypeId, AssignedToUserId
            // These are listed in the Web.Config file
            var trackedProperties = WebConfigurationManager.AppSettings["TrackedHistoryProperties"].Split(',').ToList();

            foreach (var property in newTicket.GetType().GetProperties())
            {
                // If the current property is NOT one of the properties I am interested in, then loop back (continue) and read next property
                if (!trackedProperties.Contains(property.Name))
                {
                    continue;
                }

                var oldPropValue = (property.GetValue(oldTicket, null) ?? "").ToString();  // If the property value is null, change it to empty string
                var newPropValue = (property.GetValue(newTicket, null) ?? "").ToString();  // ?? is called a "null coalescing operator"

                if (oldPropValue != newPropValue)
                {
                    var newHistory = new TicketHistory
                    {
                        Property = property.Name,
                        OldValue = Utilities.MakeReadable(property.Name, oldPropValue),
                        NewValue = Utilities.MakeReadable(property.Name, newPropValue),
                        DateChanged = newTicket.Updated.GetValueOrDefault(),
                        TicketId = newTicket.Id,
                        UserId = HttpContext.Current.User.Identity.GetUserId()
                    };
                    db.TicketHistories.Add(newHistory);
                }
            }
            db.SaveChanges();
        }
    }
}