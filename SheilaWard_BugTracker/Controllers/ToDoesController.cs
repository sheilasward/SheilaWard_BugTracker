using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_BugTracker.Controllers
{
    [RequireHttps]
    public class ToDoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: ToDo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId, Due, Task, Color")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                toDo.GrayedOut = false;
                var due = toDo.Due;
                db.ToDoes.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                TempData["Message"] = "THE MODEL STATE IS NOT VALID...";
                return RedirectToAction("Dashboard", "Home");
            }
        }

        // POST: ToDoes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            ToDo toDo = db.ToDoes.Find(id);
            db.ToDoes.Remove(toDo);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }
    }
}