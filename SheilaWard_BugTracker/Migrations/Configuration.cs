namespace SheilaWard_BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SheilaWard_BugTracker.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SheilaWard_BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SheilaWard_BugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));  // Instantiates a RoleManager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            #region Roles
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            #endregion

            #region Users
            if (!context.Users.Any(r => r.UserName == "admin@myblog.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "admin@myblog.com",
                    Email = "admin@myblog.com",
                    FirstName = "Sheila",
                    LastName = "Ward",
                    DisplayName = "Sheila Ward",
                    AvatarUrl = "~/Images/default-user-icon-8.jpg"
                }, "P@ssw0rd");
            }
            #endregion

            #region AssignToRoles
            var adminId = userManager.FindByEmail("admin@myblog.com").Id;
            userManager.AddToRole(adminId, "Admin");
            #endregion

            #region Projects
            context.Projects.AddOrUpdate(
                p => p.Name,
                    new Project { Name = "Coder Foundry Blog", Description = "Blog exercise", Created = DateTimeOffset.Now },
                    new Project { Name = "Sheila Ward Portfolio", Description = "My Portfolio created during Coder Foundry", Created = DateTimeOffset.Now },
                    new Project { Name = "Bug Tracker Project", Description = "System to track tickets in an IT system - can be bugs, or requests for enhancements or documentation", Created = DateTime.Now }
            );
            #endregion

            #region TicketPriorities
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Name = "None", Description = "Priority has not been determined"},
                    new TicketPriority { Name = "Low", Description = "Lowest priority level" },
                    new TicketPriority { Name = "Medium", Description = "Mid-level priority" },
                    new TicketPriority { Name = "High", Description = "High priority" },
                    new TicketPriority { Name = "Urgent", Description = "Highest priority level" }
            );
            #endregion

            #region TicketStatuses
            context.TicketStatuses.AddOrUpdate(
                new TicketStatus { Name = "Inactive", Description = "Has not been approved or has been tabled" },
                new TicketStatus { Name = "Active/Unassigned", Description = "Manager has approved, but not assigned to developer" },
                new TicketStatus { Name = "Active/Assigned", Description = "Developer is currently working on ticket" },
                new TicketStatus { Name = "Completed", Description = "Development and Testing done, but not deployed" },
                new TicketStatus { Name = "Archived", Description = "Ticket is completed and all work delivered"}
            );
            #endregion

            #region TicketTypes
            context.TicketTypes.AddOrUpdate(
                new TicketType { Name = "Defect", Description = "There is a defect in the application code or logic" },
                new TicketType { Name = "Documentation", Description = "There is a need for documentation/training on the application"},
                new TicketType { Name = "Enhancement", Description = "There is a request for more functionality for the application"}
            );
            #endregion
        }
    }
}
