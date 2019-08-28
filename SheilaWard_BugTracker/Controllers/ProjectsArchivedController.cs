using SheilaWard_BugTracker.Helpers;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_BugTracker.Controllers
{
    public class ProjectsArchivedController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();

        [Authorize(Roles = "Admin, ProjectManager")]
        // GET: Archived Projects List
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        // GET: Create Archived Project
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: Create Archived Project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Created,Updated")] Project project)
        {
            if (ModelState.IsValid)
            {
                var projectArchived = new ProjectArchive();
                projectArchived.Name = project.Name;
                projectArchived.Description = project.Description;
                projectArchived.Created = project.Created;
                projectArchived.Updated = project.Updated;
                projectArchived.Archived = DateTimeOffset.Now;
                db.ProjectsArchived.Add(projectArchived);
                db.SaveChanges();
                return RedirectToAction("Archive", "Projects");
            }
            else
            {
                TempData["Message"] = "SOMETHING WENT WRONG - THE MODEL STATE IS INVALID.";
                return RedirectToAction("Index", "Projects");
            }
        }

    }
}