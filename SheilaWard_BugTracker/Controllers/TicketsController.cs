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
        public ActionResult Index(string tkt)
        {
            var userId = User.Identity.GetUserId();
            ViewBag.AllDevs = new SelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName");

            //SelectList(projHelper.UsersInRoleOnProject(db.Tickets.Find(ticketId).ProjectId), "Developer");

            if (tkt == "AllTickets")
            {
                TempData["activeTab"] = "All";
                return View(db.Tickets.ToList());
            }
            //var userId = User.Identity.GetUserId();
            //    var projHelper = new ProjectsHelper();
            //    var projList = projHelper.ListUserProjects(userId);
            //    return View(projList);

 
            var tickets = tktHelper.ListOfUsersTickets();
            return View(tickets.ToList());

            //var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            //return View(tickets.ToList());
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
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTimeOffset.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatuses.AsNoTracking().FirstOrDefault(t => t.Name == "New/Unassigned").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, ProjectManager, Submitter, Developer")]
        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "Id,TicketTypeId,TicketPriorityId,TicketStatusId,AssignedToUserId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                // Get the "old" ticket from the DB before changing and rewriting
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                // Now update the ticket
                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property(x => x.TicketTypeId).IsModified = true;
                db.Entry(ticket).Property(x => x.TicketPriorityId).IsModified = true;
                db.Entry(ticket).Property(x => x.Title).IsModified = true;
                db.Entry(ticket).Property(x => x.Description).IsModified = true;
                if (ticket.AssignedToUserId != null)
                {
                    db.Entry(ticket).Property(x => x.AssignedToUserId).IsModified = true;
                }
                if (ticket.TicketStatusId != 0)
                {
                    db.Entry(ticket).Property(x => x.TicketStatusId).IsModified = true;
                }
                ticket.Updated = DateTimeOffset.Now;
                db.SaveChanges();

                // Now call the NotificationHelper to determine if Notification(s) need to be created
                notfHelper.CreateAssignmentNotification(oldTicket, ticket);

                // Now call the HistoryHelper to record changes
                histHelper.RecordHistory(oldTicket, ticket);

                // Now return to Ticket Index list
                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(projHelper.UsersInRoleOnProject(ticket.ProjectId, SystemRole.Developer), "Id", "FullName", ticket.AssignedToUserId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Dashboard
        [Authorize]
        public ActionResult Dashboard(int id)
        {
            var ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
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

                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property(x => x.AssignedToUserId).IsModified = true;
                ticket.Updated = DateTimeOffset.Now;
                var newTicket = ticket;
                db.SaveChanges();

                newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                // Now call the NotificationHelper to determine if Notification(s) need to be created
                notfHelper.CreateAssignmentNotification(oldTicket, ticket);

                //ticket.TicketStatusId = db.TicketStatuses.AsNoTracking().FirstOrDefault(t => t.Name == "Active/Assigned").Id;
                

                // Now call the NotificationHelper to determine if Notification(s) need to be created
                notfHelper.CreateAssignmentNotification(oldTicket, ticket);

                // Now call the HistoryHelper to record changes
                histHelper.RecordHistory(oldTicket, ticket);

                return RedirectToAction("Index");
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
