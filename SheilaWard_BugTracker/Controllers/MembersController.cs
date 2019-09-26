using SheilaWard_BugTracker.Helpers;
using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SheilaWard_BugTracker.Models;

namespace SheilaWard_BugTracker.Controllers
{
    [RequireHttps]
    public class MembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Members
        [Authorize]
        public ActionResult EditProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = User.Identity.GetUserId();
            }

            var member = db.Users.Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber 
            }).FirstOrDefault(u => u.Id == userId);

            return View(member);
        }

        // POST: Members
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditProfile(UserProfileViewModel member)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(member.Id);
                user.FirstName = member.FirstName;
                user.LastName = member.LastName;
                user.DisplayName = member.DisplayName;
                user.Email = member.Email;
                user.UserName = member.Email;
                user.PhoneNumber = member.PhoneNumber;

                if (ImageHelpers.IsWebFriendlyImage(member.Avatar))
                {
                    var fileName = Path.GetFileName(member.Avatar.FileName);
                    member.Avatar.SaveAs(Path.Combine(Server.MapPath("~/Avatars/"), fileName));
                    user.AvatarUrl = "/Avatars/" + fileName;
                }

                db.SaveChanges();
                if (member.Id == User.Identity.GetUserId())
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    return RedirectToAction("UserIndex", "Admin");
                }
            }
            else
            {
                return View(member);
            }
            
            
        }
    }
}