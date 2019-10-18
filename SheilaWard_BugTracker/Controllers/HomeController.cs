using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_BugTracker.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            var currentUserId = User.Identity.GetUserId();
            var userToDos = db.ToDoes.Where(t => t.UserId == currentUserId).ToList();
            ViewBag.ToDoList = "";
            foreach (var toDo in userToDos)
            {
                // Calculate datetime difference between toDo.Due and now
                DateTime currDateTime = DateTime.Now;
                System.TimeSpan diff = @toDo.Due.Subtract(currDateTime);
 
                if (diff.Days == 0 && diff.Hours == 0 && diff.Minutes == 0)
                {
                   toDo.WordDiff = "now";
                }

                else if (diff.Days < 0 || diff.Hours < 0 || diff.Minutes < 0)
                {
                    toDo.WordDiff = "overdue";
                }

                else if (diff.Days > 0)
                {
                    toDo.WordDiff = diff.Days + " day(s)";
                }
                else if (diff.Hours > 0)
                {
                    toDo.WordDiff = diff.Hours + " hour(s)";
                }
                else
                {
                    toDo.WordDiff = diff.Minutes + " minute(s)";
                }
            }
            ViewBag.ToDoList = userToDos;

            return View();
        }

        public ActionResult DemoLanding()
        {
            return View();
        }
    }
}