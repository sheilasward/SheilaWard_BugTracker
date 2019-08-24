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

        [Authorize]
        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
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
                ticket.TicketStatusId = db.TicketStatuses.AsNoTracking().FirstOrDefault(t => t.Name == "Inactive").Id;
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
                var newTicket = db.Tickets.Find(ticket.Id);
                newTicket.TicketTypeId = ticket.TicketTypeId;
                newTicket.TicketPriorityId = ticket.TicketPriorityId;
                newTicket.Title = ticket.Title;
                newTicket.Description = ticket.Description;
                newTicket.Updated = DateTimeOffset.Now;
                if (ticket.AssignedToUserId != null)
                {
                    newTicket.AssignedToUserId = ticket.AssignedToUserId;
                }
                if (ticket.TicketStatusId != 0)
                {
                    newTicket.TicketStatusId = ticket.TicketStatusId;
                }
                db.SaveChanges();
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
            return View(db.Tickets.Find(id));
        }

        // GET: AssignToTkt (Assign Users to Tickets) - Admins can access this for all Users.
        // PMs can access this for Developers only
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignToTkt()
        {
            return View();
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
