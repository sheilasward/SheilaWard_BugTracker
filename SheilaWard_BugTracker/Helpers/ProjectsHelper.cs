using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SheilaWard_BugTracker.Enumerations;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public class ProjectsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public List<ApplicationUser> UsersInRoleOnProject(int projectId, SystemRole roleName)
        {
            var people = new List<ApplicationUser>();
            foreach (var user in UsersOnProject(projectId).ToList())
            {
                if (roleHelper.IsUserInSystemRole(user.Id, roleName))
                {
                    people.Add(user);
                }
            }
            return people;
        }

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }
        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        public int CountUserProjects()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.Count();
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);
                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);
                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
    }
}