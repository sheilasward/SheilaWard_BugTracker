using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.Enumerations;
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
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected UserRolesHelper roleHelper = new UserRolesHelper();
        protected ProjectsHelper projectHelper = new ProjectsHelper();
        protected ApplicationUser currentUser = null;
        protected SystemRole currentRole = SystemRole.None;

        protected CommonHelper()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            if (userId != null)
            {
                currentUser = db.Users.Find(userId);
                var stringRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                if (!string.IsNullOrEmpty(stringRole))
                    currentRole = (SystemRole)Enum.Parse(typeof(SystemRole), stringRole);
            }
            
        }
    }
}