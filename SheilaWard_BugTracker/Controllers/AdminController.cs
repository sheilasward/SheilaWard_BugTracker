using SheilaWard_BugTracker.Helpers;
using SheilaWard_BugTracker.Models;
using SheilaWard_BugTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_BugTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        // GET: Admin
        public ActionResult UserIndex()
        {
            var roles = db.Roles.ToList();
            var projects = db.Projects.ToList();
            var users = db.Users.Select(u => new UserIndexViewModel
            {
                Id = u.Id,
                AvatarUrl = u.AvatarUrl,
                FullName = u.FirstName + " " + u.LastName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            }).ToList();

            foreach (var user in users)
            {
                user.CurrentRole = new SelectList(roles, "Name", "Name", roleHelper.ListUserRoles(user.Id).FirstOrDefault());
                //user.CurrentProjects = new MultiSelectList(projects, "Id", "Name", projectHelper.ListUserProjects(user.Id));
            }

            return View(users);
        }

        public ActionResult ManageRoles()
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
            ViewBag.RoleName = new SelectList(db.Roles.ToList(), "Name", "Name");

            return View(users);
        }

        [HttpPost]
        public ActionResult ManageRoles(List<string> users, string roleName)
        {
           if (users != null)
            {
                // Iterate over the incoming list of Users that were selected from the form
                foreach (var userId in users)
                {
                    // Remove each of them from whatever role they occupy currently
                    foreach (var role in roleHelper.ListUserRoles(userId))
                    {
                        roleHelper.RemoveUserFromRole(userId, role);
                    }
                    // Then add them back to the selected role
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        roleHelper.AddUserToRole(userId, roleName);
                    }
                }
            }

            return RedirectToAction("ManageRoles");
        }

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
            ViewBag.ProjectName = new SelectList(db.Projects.ToList(), "Name", "Name");

            return View(users);
        }

        [HttpPost]
        public ActionResult ManageProjects(List<string> users, int projectId)
        {
            if (users != null)
            {
                // Iterate over the incoming list of Users that were selected from the form
                foreach (var userId in users)
                {
                    // Remove each of them from whatever role they occupy currently
                    foreach (var project in projectHelper.ListUserProjects(userId))
                    {
                        projectHelper.RemoveUserFromProject(userId, projectId);
                    }
                    //// Then add them back to the selected role
                    //if (projectId = null)
                    //if (!int.IsNull(projectId))
                    //{
                    //    projectHelper.AddUserToProject(userId, projectId);
                    //}
                }
            }

            return RedirectToAction("ManageProjects");
        }

        public ActionResult ManageUserRole(string userId)
        {
            var currentRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            ViewBag.UserId = userId;
            ViewBag.UserRole = new SelectList(db.Roles.ToList(), "Name", "Name", currentRole);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUserRole(string userId, string userRole)
        {
            // Remove the incoming user from all roles they currently occupy
            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }
            // If the incoming role selection is not null, assign the user to the selected role
            if (!string.IsNullOrEmpty(userRole))
            {
                roleHelper.AddUserToRole(userId, userRole);
            }

            return RedirectToAction("ManageUserRole");
        }

        public ActionResult ManageUserProjects(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
    }
}