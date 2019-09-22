using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Models;
using SheilaWard_BugTracker.Helpers;
using SheilaWard_BugTracker.Enumerations;

namespace SheilaWard_BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private TicketDecisionHelper tktHelper = new TicketDecisionHelper();
        private NotificationHelper notfHelper = new NotificationHelper();
        private HistoryHelper histHelper = new HistoryHelper();

        [Authorize]
        // GET: Tickets
        public ActionResult Index(string tkt, string stat)
        {
            var userId = User.Identity.GetUserId();
            ViewBag.AllDevs = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");
            ViewBag.Stats = stat;

            if (tkt == "AllTickets")
            {
                TempData["activeTab"] = "All";
                return View(db.Tickets.ToList());
            }

            var tickets = tktHelper.ListOfUsersTickets();
            return View(tickets.ToList());
        }

        [Authorize]
        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Dashboard
        [Authorize(Roles = "Admin, ProjectManager, Submitter, Developer")]
        public ActionResult Dashboard(int id)
        {
            var ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var project = ticket.Project;
            ViewBag.TeamPMs = projHelper.UsersInRoleOnProject(project.Id, SystemRole.ProjectManager).ToList();
            ViewBag.TeamSubs = projHelper.UsersInRoleOnProject(project.Id, SystemRole.Submitter).ToList();
            ViewBag.TeamDevs = projHelper.UsersInRoleOnProject(project.Id, SystemRole.Developer).ToList();
            ViewBag.History = ticket.TicketHistories;
            ViewBag.CurrentUser = User.Identity.GetUserId();
            ViewBag.DevProjects = projHelper.ListUserProjects(ticket.AssignedToUserId);
            return View(ticket);

        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.ProjectId = new SelectList(db.Users.Find(userId).Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind Include gets info from the form
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description,PercentComplete,Archived")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.PercentComplete = 0;
                ticket.TicketStatusId = db.TicketStatuses.AsNoTracking().FirstOrDefault(t => t.Name == "New/Unassigned").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();

                // Now call the NotificationHelper to send a notification to every manager on the project of the new ticket.
                notfHelper.NewTicketNotification(ticket);

                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, ProjectManager, Submitter, Developer")]
        public ActionResult Edit(int? id, string stat)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
 
            if (ticket == null)
            {
                return HttpNotFound();
            }

            // if the method is not "static", you have to make a new instance of the decision helper
            var decisionHelper = new TicketDecisionHelper();
            if (decisionHelper.TicketIsEditableByUser(ticket))
            {
                ViewBag.AssignedToUserId = new SelectList(projHelper.UsersInRoleOnProject(ticket.ProjectId, SystemRole.Developer), "Id", "FullName", ticket.AssignedToUserId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                ViewBag.Stats = stat;
                ViewBag.CurrentUser = User.Identity.GetUserId();
                return View(ticket);
            }
            else
            {
                TempData["Message"] = "YOU ARE NOT AUTHORIZED TO EDIT THIS TICKET BASED ON YOUR ASSIGNED ROLE.";
                return RedirectToAction("Index", "Tickets");
            }
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketStatusId,TicketTypeId,TicketPriorityId,AssignedToUserId,Title,Description,PercentComplete,Archived")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // Get the "old" ticket from the DB before changing and rewriting
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                var newTicket = db.Tickets.Find(ticket.Id);
                newTicket.AssignedToUserId = ticket.AssignedToUserId;
                newTicket.TicketStatusId = ticket.TicketStatusId;
                newTicket.TicketTypeId = ticket.TicketTypeId;
                newTicket.TicketPriorityId = ticket.TicketPriorityId;
                newTicket.AssignedToUserId = ticket.AssignedToUserId;
                newTicket.Title = ticket.Title;
                newTicket.Description = ticket.Description;
                newTicket.PercentComplete = ticket.PercentComplete;
                if (newTicket.PercentComplete == 100)
                {
                    newTicket.TicketStatusId = db.TicketStatuses.AsNoTracking().FirstOrDefault(t => t.Name == "Completed").Id;
                }
                newTicket.Updated = DateTimeOffset.Now;
                if (newTicket.TicketStatusId == db.TicketStatuses.AsNoTracking().FirstOrDefault(t => t.Name == "Withdrawn").Id)
                {
                    newTicket.AssignedToUserId = null;
                }

                // Boolean Variables to automatically set status
                var noChange = (oldTicket.AssignedToUserId == newTicket.AssignedToUserId);
                var assignment = (string.IsNullOrEmpty(oldTicket.AssignedToUserId));
                var unassignment = (string.IsNullOrEmpty(newTicket.AssignedToUserId));

                var ActiveStatusId = db.TicketStatuses.FirstOrDefault(ts => ts.Name == "Active/Assigned").Id;
                var InactiveStatusId = db.TicketStatuses.FirstOrDefault(ts => ts.Name == "Inactive").Id;

                if (assignment) newTicket.TicketStatusId = ActiveStatusId;
                if (unassignment) newTicket.TicketStatusId = InactiveStatusId;

                db.SaveChanges();

                // Now call the NotificationHelper to determine if Notification(s) need to be created
                notfHelper.ManageNotifications(oldTicket, newTicket);

                // Now call the HistoryHelper to record changes
                histHelper.RecordHistory(oldTicket, newTicket);

                // Now return:
                return RedirectToAction("Index", "Tickets", new { proj = "MyTickets", stat = "InProg" });

            }
            ViewBag.AssignedToUserId = new SelectList(projHelper.UsersInRoleOnProject(ticket.ProjectId, SystemRole.Developer), "Id", "FullName", ticket.AssignedToUserId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.CurrentUser = User.Identity.GetUserId();
            return View(ticket);
        }


        [Authorize(Roles = "Admin,ProjectManager")]
        // GET: Tickets/Archive/5
        public ActionResult Archive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed([Bind(Include = "Id,TicketTypeId,TicketPriorityId,TicketStatusId,AssignedToUserId,Title,Description,PercentComplete,Archived")] int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            ticket.Archived = true;
            var ArchivedStatusId = db.TicketStatuses.FirstOrDefault(ts => ts.Name == "Archived").Id;
            ticket.TicketStatusId = ArchivedStatusId;
            db.SaveChanges();
            return RedirectToAction("Index", "Tickets", new { proj = "MyTickets", stat = "Completed" }); ;
        }

       

        // GET: AssignToTkt (Assign Developers to Tickets) - Admins can access this for all Developers.
        // PMs can access this for Developers only on the Project Manager's projects.
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignToTkt(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(Id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var mgrId = User.Identity.GetUserId();
            var manager = db.Users.Find(mgrId);
            var mgrName = manager.FullName;

            ViewBag.Manager = mgrName;
            ViewBag.AssignedToUserId = new SelectList(projHelper.UsersInRoleOnProject(ticket.ProjectId, SystemRole.Developer), "Id", "FullName", ticket.AssignedToUserId);
            return View(ticket);
        }

        // POST: AssignToTkt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToTkt([Bind(Include = "Id,TicketTypeId,TicketPriorityId,TicketStatusId,AssignedToUserId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // Get the "old" ticket from the DB before changing and rewriting
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                var newTicket = db.Tickets.Find(ticket.Id);
                newTicket.AssignedToUserId = ticket.AssignedToUserId;
                newTicket.Updated = DateTimeOffset.Now;

                // Boolean Variables to automatically set status
                var noChange = (oldTicket.AssignedToUserId == newTicket.AssignedToUserId);
                var assignment = (string.IsNullOrEmpty(oldTicket.AssignedToUserId));
                var unassignment = (string.IsNullOrEmpty(newTicket.AssignedToUserId));

                var ActiveStatusId = db.TicketStatuses.FirstOrDefault(ts => ts.Name == "Active/Assigned").Id;
                var InactiveStatusId = db.TicketStatuses.FirstOrDefault(ts => ts.Name == "Inactive").Id;

                if (assignment) newTicket.TicketStatusId = ActiveStatusId;
                if (unassignment) newTicket.TicketStatusId = InactiveStatusId;

                db.SaveChanges();

                //db.Tickets.Attach(ticket);
                //db.Entry(ticket).Property(x => x.AssignedToUserId).IsModified = true;
                //ticket.Updated = DateTimeOffset.Now;
                //var newTicket = ticket;
                //db.SaveChanges();

                // Now call the NotificationHelper to determine if Notification(s) need to be created
                notfHelper.ManageNotifications(oldTicket, newTicket);

                // Now call the HistoryHelper to record changes
                histHelper.RecordHistory(oldTicket, newTicket);

                return RedirectToAction("Index", "Tickets", new { proj = "MyTickets", stat = "InProg" });
            }

            var mgrId = User.Identity.GetUserId();
            var manager = db.Users.Find(mgrId);
            var mgrName = manager.FullName;

            ViewBag.Manager = mgrName;
            ViewBag.AssignedToUserId = new SelectList(projHelper.UsersInRoleOnProject(ticket.ProjectId, SystemRole.Developer), "Id", "FullName", ticket.AssignedToUserId);
            return View(ticket);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
