using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Helpers;
using SheilaWard_BugTracker.Models;

namespace SheilaWard_BugTracker.Controllers
{
    [RequireHttps]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationHelper notfHelper = new NotificationHelper();

        // GET: TicketAttachments
        //public ActionResult Index()
        //{
        //    var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
        //    return View(ticketAttachments.ToList());
        //}

        //// GET: TicketAttachments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
        //    if (ticketAttachment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketAttachment);
        //}

        // GET: TicketAttachments/Create
        //public ActionResult Create()
        //{
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
        //    return View();
        //}

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId")] TicketAttachment ticketAttachment, string attachmentTitle, string attachmentDescription, HttpPostedFileBase attachment)
        {
            if (ModelState.IsValid)
            {
                ticketAttachment.Title = attachmentTitle;
                ticketAttachment.Description = attachmentDescription;
                ticketAttachment.Created = DateTimeOffset.Now;
                ticketAttachment.UserId = User.Identity.GetUserId();

                if (ImageHelpers.IsValidAttachment(attachment))
                {
                    var fileName = Path.GetFileNameWithoutExtension(attachment.FileName);
                    var fileExtension = Path.GetExtension(attachment.FileName);
                    // 1. Add DateTime stamp to end of filename
                    // 2. Run that through slugmaker
                    // 3. Map path
                    // 4. Save complete file
                    var fileWithDate = $"{fileName}-{DateTime.Now}";
                    var slugFileName = Utilities.CreateSlug(fileWithDate);
                    var formattedMedia = $"{slugFileName}{fileExtension}";
                    attachment.SaveAs(Path.Combine(Server.MapPath("~/Attachments/"), formattedMedia));
                    ticketAttachment.AttachmentUrl = "/Attachments/" + formattedMedia;
                }
                else
                {
                    TempData["Message"] = "THIS ATTACHMENT IS NOT VALID...";
                    return RedirectToAction("Dashboard", "Tickets", new {id = ticketAttachment.TicketId });
                }

                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();

                // Now call the NotificationHelper to send a notification to the developer
                notfHelper.NotifyDevTA(ticketAttachment);

                return RedirectToAction("Dashboard", "Tickets", new { id = ticketAttachment.TicketId });
            }

            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,Description,Created,AttachmentUrl")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "OwnerUserId", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
