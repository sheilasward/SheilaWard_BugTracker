using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    // abstract means that this class cannot be instantiated
    public abstract class CommonHelper
    {
        // protected means only my children who inherit from me can see these
        protected static ApplicationDbContext db = new ApplicationDbContext();
        protected static UserRolesHelper roleHelper = new UserRolesHelper();
        protected static ProjectsHelper projectHelper = new ProjectsHelper();

        protected ApplicationUser currentUser = null;
        protected String currentRole = "";

        protected CommonHelper()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            this.currentUser = db.Users.Find(userId);
            this.currentRole = roleHelper.ListUserRoles(this.currentUser.Id).FirstOrDefault();
        }
    }
}