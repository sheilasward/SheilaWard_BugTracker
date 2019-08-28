﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Helpers;
using SheilaWard_BugTracker.Models;
using SheilaWard_BugTracker.ViewModels;

namespace SheilaWard_BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();

        [Authorize(Roles = "Admin, ProjectManager, Submitter, Developer")]
        // GET: Projects
        public ActionResult Index(string proj)
        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;

            if (proj == "AllProjects")
            {
                TempData["activeTab"] = "All";
                return View(db.Projects.ToList());
            }
            else
            {
                var projHelper = new ProjectsHelper();
                var projList = projHelper.ListUserProjects(userId);
                return View(projList);
            }
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var newUser = db.Users.Find(userId);
                project.Users.Add(newUser);
                project.Created = DateTimeOffset.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Created")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Updated = DateTimeOffset.Now;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Projects/AssignToProj (Assign Users to Projects) - Admins and PMs can access this for all Users. 
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AssignToProj(int? id)
        {
            if (id == null)
            {
                // pass to view list of all projects for drop-down
                var projList = new List<Project>();
                ViewBag.Projects = new MultiSelectList(projList);
                return View(projList);
            }
            // pass the 1 project to view
            Project project = db.Projects.Find(id);
            return View(project);

        }

        // GET Projects/ManageProjects
        public ActionResult ManageProjects()
        {
            var users = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl,
                Email = u.Email
            }).ToList();

            ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullNameWithEmail");
            ViewBag.ProjectName = new MultiSelectList(db.Projects.ToList(), "Id", "Name");

            return View(users);
        }

        // POST Projects/ManageProjects 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjects(List<string> users, List<int> projectName)
        {
            if (users != null)
            {
                // Iterate over the incoming list of Users that were selected from the form
                foreach (var userId in users)
                {
                    // Remove each of them from whatever role they occupy currently
                    foreach (var project in projHelper.ListUserProjects(userId))
                    {
                        projHelper.RemoveUserFromProject(userId, project.Id);
                    }
                    //Then add them back to the selected project
                    if (projectName != null)
                    {
                        foreach (var project in projectName)
                        {
                            projHelper.AddUserToProject(userId, project);
                        }

                    }
                }
            }

            return RedirectToAction("ManageProjects");
        }

        // GET: Projects/Archive
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Archive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult Archive(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Projects/Archive
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Archive([Bind(Include = "Id,Name,Description,Created,Updated")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
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
