using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SheilaWard_BugTracker.Helpers
{
    public class NotificationHelper : CommonHelper
    {
        public void CreateAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            // 4 possible cases
            // 1. No change in assignment
            // 2. Assignment - AssignedToUserId was null, but now is assigned
            // 3. Unassignment - AssignedToUserId was assigned, but now it is not
            // 4. Reassignment - AssignedToUserId was assigned, but now it is assigned to another developer

            // Boolean variables:
            var noChange = (oldTicket.AssignedToUserId == newTicket.AssignedToUserId);
            var assignment = (string.IsNullOrEmpty(oldTicket.AssignedToUserId));
            var unassignment = (string.IsNullOrEmpty(newTicket.AssignedToUserId));

            if (noChange) return;
            if (assignment) GenerateAssignmentNotification(oldTicket, newTicket);
            else if (unassignment) GenerateUnassignmentNotification(oldTicket, newTicket);
            else
            {
                GenerateAssignmentNotification(oldTicket, newTicket);
                GenerateUnassignmentNotification(oldTicket, newTicket);
            }

        } 
        private void GenerateAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                Created = DateTimeOffset.Now,
                Subject = $"You were assigned to Ticket Id {newTicket.Id} on {DateTime.Now}",
                IsRead = false,
                RecipientId = newTicket.AssignedToUserId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = $"Please acknowledge that you have read this notification by marking it as 'READ'.",
                TicketId = newTicket.Id
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();

        }
        private void GenerateUnassignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotification
            {
                Created = DateTimeOffset.Now,
                Subject = $"You were unassigned from Ticket Id {newTicket.Id} on {DateTime.Now}",
                IsRead = false,
                RecipientId = oldTicket.AssignedToUserId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = $"Please acknowledge that you have read this notification by marking it as 'READ'.",
                TicketId = newTicket.Id
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        public int GetNewUserNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(t => t.RecipientId == userId && !t.IsRead).Count();
        }

        public int GetAllUserNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(t => t.RecipientId == userId).Count();
        }

        public List<TicketNotification> GetUnreadUserNotifications()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(t => t.RecipientId == userId && !t.IsRead).ToList();
        }
    }
}