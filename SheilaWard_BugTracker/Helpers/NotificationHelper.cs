using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Enumerations;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace SheilaWard_BugTracker.Helpers
{
    public class NotificationHelper : CommonHelper
    {
        private ProjectsHelper projHelper = new ProjectsHelper();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            CreateAssignmentNotification(oldTicket, newTicket);
            CreateChangeNotification(oldTicket, newTicket);
        }
        private void CreateAssignmentNotification(Ticket oldTicket, Ticket newTicket)
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
            var senderId = HttpContext.Current.User.Identity.GetUserId();
            var notification = new TicketNotification
            {
                Created = DateTime.Now,
                Subject = $"You were assigned to Ticket '{newTicket.Title}' on {DateTime.Now.ToString("M/d/yyyy mm:hhtt")}",
                IsRead = false,
                RecipientId = newTicket.AssignedToUserId,
                SenderId = senderId,
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
                Created = DateTime.Now,
                Subject = $"You were unassigned from Ticket '{newTicket.Title}' on {DateTime.Now.ToString("M/d/yyyy mm:hhtt")}",
                IsRead = false,
                RecipientId = oldTicket.AssignedToUserId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = $"Please acknowledge that you have read this notification by marking it as 'READ'.",
                TicketId = newTicket.Id
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        private void CreateChangeNotification(Ticket oldTicket, Ticket newTicket)
        {
            var messageBody = new StringBuilder();
            foreach (var property in WebConfigurationManager.AppSettings["TrackedTicketProperties"].Split(','))
            {
                var oldValue = Utilities.MakeReadable(property, oldTicket.GetType().GetProperty(property).GetValue(oldTicket, null).ToString());
                var newValue = Utilities.MakeReadable(property, newTicket.GetType().GetProperty(property).GetValue(newTicket, null).ToString());

                if (oldValue != newValue)
                {
                    if (property == "PercentComplete" && newTicket.PercentComplete == 100) CompleteNotification(newTicket);
                    messageBody.AppendLine(new String('-', 45));
                    messageBody.AppendLine($"A change was made to Property: {property}.");
                    messageBody.AppendLine($"The old value was: {oldValue.ToString()}");
                    messageBody.AppendLine($"The new value is: {newValue.ToString()}");
                }
            }
            if (!string.IsNullOrEmpty(messageBody.ToString()))
            {
                var message = new StringBuilder();
                message.AppendLine($"Changes were made to '{newTicket.Title}' on {newTicket.Updated.GetValueOrDefault().ToString("MMM d, yyyy")}");
                message.AppendLine(messageBody.ToString());
                var senderId = HttpContext.Current.User.Identity.GetUserId();

                var notification = new TicketNotification
                {
                    TicketId = newTicket.Id,
                    Created = DateTime.Now,
                    Subject = $"Ticket with title '{newTicket.Title}' has changed",
                    RecipientId = newTicket.AssignedToUserId,
                    SenderId = senderId,
                    NotificationBody = message.ToString(),
                    IsRead = false
                };

                db.TicketNotifications.Add(notification);
                db.SaveChanges();
            }
        }

        public void NewTicketNotification(Ticket ticket)

        {
            List<ApplicationUser> recipients = new List<ApplicationUser>();

            recipients = projHelper.UsersInRoleOnProject(ticket.ProjectId, SystemRole.ProjectManager).ToList();
            foreach (var recipient in recipients)
            {
                GeneratePMNotification(recipient, ticket);
            }
        }

        private void GeneratePMNotification(ApplicationUser recipient, Ticket ticket)
        {
            var notification = new TicketNotification
            {
                Created = DateTime.Now,
                Subject = $"A new Ticket '{ticket.Title}' needs assignment",
                IsRead = false,
                RecipientId = recipient.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = $"Please acknowledge that you have read this notification by marking it as 'READ'.",
                TicketId = ticket.Id
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        private void CompleteNotification(Ticket ticket)
        {
            var project = ticket.Project;
            var managers = projHelper.UsersInRoleOnProject(project.Id, SystemRole.ProjectManager).ToList();
            foreach (var manager in managers)
            {
                var notification = new TicketNotification
                {
                    TicketId = ticket.Id,
                    Created = DateTime.Now,
                    Subject = $"Ticket has been marked complete",
                    RecipientId = manager.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    NotificationBody = $"Ticket with title '{ticket.Title}' is now complete, and can be archived.",
                    IsRead = false
                };

                db.TicketNotifications.Add(notification);
                db.SaveChanges();
            }
        }

        public void NotifyDevTA(TicketAttachment ticketAttachment)
        {
            var ticketId = ticketAttachment.TicketId;
            var ticket = db.Tickets.Find(ticketId);
            var messageBody = new StringBuilder();
            messageBody.AppendLine($"Ticket Title:  {ticket.Title}");
            messageBody.AppendLine(new String('-', 45));
            messageBody.AppendLine($"Attachment Title:  {ticketAttachment.Title}");
            messageBody.AppendLine($"Attachment Description:  {ticketAttachment.Description}");
            messageBody.AppendLine(@ImageHelpers.GetIconPath(ticketAttachment.AttachmentUrl));
            
            var notification = new TicketNotification
            {
                Created = DateTime.Now,
                Subject = $"A new Attachment was uploaded on {DateTime.Now.ToString("M/d/yyyy mm:hhtt")}",
                IsRead = false,
                RecipientId = ticket.AssignedToUserId,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                NotificationBody = messageBody.ToString(),
                TicketId = ticket.Id
            };

            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        public int GetNewUserNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(t => t.RecipientId == userId && !t.IsRead).Count();
        }

        public int GetReadUserNotificationCount()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Where(t => t.RecipientId == userId && t.IsRead).Count();
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