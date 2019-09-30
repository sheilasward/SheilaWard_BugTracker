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

namespace SheilaWard_BugTracker.Controllers
{
    [RequireHttps]
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private TicketDecisionHelper decisionHelper = new TicketDecisionHelper();

        // GET: TicketComments
        public ActionResult Index()
        {
            var ticketComments = db.TicketComments.Include(t => t.Author).Include(t => t.Ticket);
            return View(ticketComments.ToList());
        }

        // GET: TicketComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // GET: TicketComments/Create
        //public ActionResult Create()
        //{
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId");
        //    return View();
        //}

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId, Comment")] TicketComment ticketComment, string Comment) 
        {
            if (ModelState.IsValid)
            {
                ticketComment.Comment = Comment;
                ticketComment.Created = DateTimeOffset.Now;
                ticketComment.AuthorId = User.Identity.GetUserId();
                db.TicketComments.Add(ticketComment);
                db.SaveChanges();

                // Now call the NotificationHelper to create notification
                var ticket = db.Tickets.Find(ticketComment.TicketId);
                if (ticket.AssignedToUserId != null)
                {
                    notificationHelper.NewTicketComment(ticketComment);
                }

                return RedirectToAction("Dashboard", "Tickets", new { id = ticketComment.TicketId });
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketComment.AuthorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Edit/5
        public ActionResult Edit(int? id, string stat)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            if (decisionHelper.TicketIsEditableByUser(ticketComment.Ticket))
            {
                if (ticketComment.AuthorId == userId || User.IsInRole("Administrator"))
                {
                    ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketComment.AuthorId);
                    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketComment.TicketId);
                    ViewBag.Stats = stat;
                    return View(ticketComment);
                }
                else
                {
                    TempData["Message"] = "YOU CANNOT EDIT THIS COMMENT IF YOU ARE NOT THE AUTHOR OR AN ADMINISTRATOR.";
                    ViewBag.Stats = stat;
                    return RedirectToAction("Dashboard", "Tickets", new { id = ticketComment.TicketId, stat = stat });
                }
            }
            else
            {
                TempData["Message"] = "YOU ARE NOT AUTHORIZED TO EDIT THIS COMMENT BASED ON YOUR ASSIGNED ROLE.";
                ViewBag.Stats = stat;
                return RedirectToAction("Dashboard", "Tickets", new { id = ticketComment.TicketId, stat = stat });
            }
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,AuthorId,Comment,Created")] TicketComment ticketComment, string stat)
        {
            ViewBag.Stats = stat;
            if (ModelState.IsValid)
            {
                db.Entry(ticketComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Tickets", new { id = ticketComment.TicketId, stat = stat });
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketComment.AuthorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketComment ticketComment = db.TicketComments.Find(id);
        //    if (ticketComment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketComment);
        //}

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string stat)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            var ticketId = ticketComment.TicketId;
            db.TicketComments.Remove(ticketComment);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Tickets", new { id = ticketId, stat = stat });
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
