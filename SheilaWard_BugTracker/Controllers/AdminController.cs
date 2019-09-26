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
    [RequireHttps]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        [Authorize(Roles = "Admin")]
        // GET: UserIndex - only Admins can access this.
        public ActionResult UserIndex()
        {
            var roles = db.Roles.ToList();
            var projects = db.Projects.ToList();
            var users = db.Users.Select(u => new UserIndexViewModel
            {
                Id = u.Id,
                AvatarUrl = u.AvatarUrl,
                FullName = u.LastName + ", " + u.FirstName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email
            }).ToList();

            foreach (var user in users)
            {
                user.CurrentRole = new SelectList(roles, "Name", "Name", roleHelper.ListUserRoles(user.Id).FirstOrDefault());
                user.CurrentProjects = new MultiSelectList(projects, "Id", "Name", projectHelper.ListUserProjects(user.Id).Select(u => u.Id));
            }

            return View(users);
        }

        // POST: UserIndex
        [HttpPost]
        public ActionResult UserIndex(string userId, string CurrentRole, List<int> CurrentProjects)
        {
            // Remove user from whatever roles they occupy currently
            // roleHelper.RemoveUserFromRole(userId, roleHelper.ListUserRoles(userId).FirstOrDefault());

            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            if (!string.IsNullOrEmpty(CurrentRole))
            {
                 // Then add them back to the selected role
                roleHelper.AddUserToRole(userId, CurrentRole);
            }

            if (CurrentProjects != null)
            {
                // Remove user from whatever projects they occupy currently
                foreach (var project in projectHelper.ListUserProjects(userId))
                {
                    projectHelper.RemoveUserFromProject(userId, project.Id);
                }
                // Then add them back to the selected role
                foreach (var project in CurrentProjects) 
                {
                    projectHelper.AddUserToProject(userId, project);
                }
            }
            return RedirectToAction("UserIndex");
        }
    }
}


// No longer used:
//public ActionResult ManageRoles()
//{
//    var users = db.Users.Select(u => new UserProfileViewModel
//    {
//        Id = u.Id,
//        FirstName = u.FirstName,
//        LastName = u.LastName,
//        DisplayName = u.DisplayName,
//        AvatarUrl = u.AvatarUrl,
//        Email = u.Email
//    }).ToList();

//    ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullNameWithEmail");
//    ViewBag.RoleName = new SelectList(db.Roles.ToList(), "Name", "Name");

//    return View(users);
//}

//[HttpPost]
//public ActionResult ManageRoles(List<string> users, string roleName)
//{
//    if (users != null)
//    {
//        // Iterate over the incoming list of Users that were selected from the form
//        foreach (var userId in users)
//        {
//            // Remove each of them from whatever role they occupy currently
//            foreach (var role in roleHelper.ListUserRoles(userId))
//            {
//                roleHelper.RemoveUserFromRole(userId, role);
//            }
//            // Then add them back to the selected role
//            if (!string.IsNullOrEmpty(roleName))
//            {
//                roleHelper.AddUserToRole(userId, roleName);
//            }
//        }
//    }

//    return RedirectToAction("ManageRoles");
//}

//public ActionResult ManageProjects()
//{
//    var users = db.Users.Select(u => new UserProfileViewModel
//    {
//        Id = u.Id,
//        FirstName = u.FirstName,
//        LastName = u.LastName,
//        DisplayName = u.DisplayName,
//        AvatarUrl = u.AvatarUrl,
//        Email = u.Email
//    }).ToList();

//    ViewBag.Users = new MultiSelectList(db.Users.ToList(), "Id", "FullNameWithEmail");
//    ViewBag.ProjectName = new MultiSelectList(db.Projects.ToList(), "Id", "Name");

//    return View(users);
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult ManageProjects(List<string> users, List<int> projectName)
//{
//    if (users != null)
//    {
//        // Iterate over the incoming list of Users that were selected from the form
//        foreach (var userId in users)
//        {
//            // Remove each of them from whatever role they occupy currently
//            foreach (var project in projectHelper.ListUserProjects(userId))
//            {
//                projectHelper.RemoveUserFromProject(userId, project.Id);
//            }
//            //Then add them back to the selected project
//            if (projectName != null)
//            {
//                foreach (var project in projectName)
//                {
//                    projectHelper.AddUserToProject(userId, project);
//                }

//            }
//        }
//    }

//    return RedirectToAction("ManageProjects");
//}

//public ActionResult ManageUserRole(string userId)
//{
//    var currentRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
//    ViewBag.UserId = userId;
//    ViewBag.UserRole = new SelectList(db.Roles.ToList(), "Name", "Name", currentRole);
//    return View();
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult ManageUserRole(string userId, string userRole)
//{
//    // Remove the incoming user from all roles they currently occupy
//    foreach (var role in roleHelper.ListUserRoles(userId))
//    {
//        roleHelper.RemoveUserFromRole(userId, role);
//    }
//    // If the incoming role selection is not null, assign the user to the selected role
//    if (!string.IsNullOrEmpty(userRole))
//    {
//        roleHelper.AddUserToRole(userId, userRole);
//    }

//    return RedirectToAction("ManageUserRole");
//}

//public ActionResult ManageUserProjects(string userId)
//{
//    if (String.IsNullOrWhiteSpace(userId))
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    ApplicationUser user = db.Users.Find(userId);
//    if (user == null)
//    {
//        return HttpNotFound();
//    }
//    return View(user);
//}