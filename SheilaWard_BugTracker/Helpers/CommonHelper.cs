using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public abstract class CommonHelper
    {
        protected static ApplicationDbContext db = new ApplicationDbContext();
        protected static UserRolesHelper roleHelper = new UserRolesHelper();
        protected static ProjectsHelper projectHelper = new ProjectsHelper();
    }
}