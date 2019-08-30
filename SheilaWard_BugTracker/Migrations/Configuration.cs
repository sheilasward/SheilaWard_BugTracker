namespace SheilaWard_BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SheilaWard_BugTracker.Helpers;
    using SheilaWard_BugTracker.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;

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
            if (!context.Users.Any(r => r.UserName == "AngieSmith@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "AngieSmith@mailinator.com",
                    Email = "AngieSmith@mailinator.com",
                    FirstName = "Angela",
                    LastName = "Smith",
                    DisplayName = "ADSmith",
                    AvatarUrl = "~/Avatars/AngieSmith.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "AresEller@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "AresEller@mailinator.com",
                    Email = "AresEller@mailinator.com",
                    FirstName = "Ares",
                    LastName = "Eller",
                    DisplayName = "AEller",
                    AvatarUrl = "~/Avatars/AresEller.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "DRose@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DRose@mailinator.com",
                    Email = "DRose@mailinator.com",
                    FirstName = "Dave",
                    LastName = "Rose",
                    DisplayName = "DRose",
                    AvatarUrl = "~/Avatars/DaveRose.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "CTriplett@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "CTriplett@mailinator.com",
                    Email = "CTriplett@mailinator.com",
                    FirstName = "Cynthia",
                    LastName = "Triplett",
                    DisplayName = "CCTriplett",
                    AvatarUrl = "~/Avatars/Cindy.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "JCooley@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "JCooley@Mailinator.com",
                    Email = "JCooley@Mailinator.com",
                    FirstName = "James",
                    LastName = "Cooley",
                    DisplayName = "JCooley",
                    AvatarUrl = "~/Avatars/cooley.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "LConnelly@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "LConnelly@Mailinator.com",
                    Email = "LConnelly@Mailinator.com",
                    FirstName = "Lee",
                    LastName = "Connelly",
                    DisplayName = "LConnelly",
                    AvatarUrl = "~/Avatars/LeeConnelly.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "SPatel@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "SPatel@Mailinator.com",
                    Email = "SPatel@Mailinator.com",
                    FirstName = "Sarvesh",
                    LastName = "Patel",
                    DisplayName = "SPatel",
                    AvatarUrl = "~/Avatars/sarvesh.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "Sheila.Ward@email.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Sheila.Ward@email.com",
                    Email = "Sheila.Ward@email.com",
                    FirstName = "Sheila",
                    LastName = "Ward",
                    DisplayName = "SSWard",
                    AvatarUrl = "~/Avatars/atReunion.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "Sherri@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Sherri@Mailinator.com",
                    Email = "Sherri@Mailinator.com",
                    FirstName = "Sherri",
                    LastName = "Creech",
                    DisplayName = "SCreech",
                    AvatarUrl = "~/Avatars/SCreech.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "TShorter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TShorter@Mailinator.com",
                    Email = "TShorter@Mailinator.com",
                    FirstName = "Teresa",
                    LastName = "Shorter",
                    DisplayName = "TShorter",
                    AvatarUrl = "~/Avatars/teresa.jpg"
                }, "P@ssw0rd");
            }
            //Introduce my Demo Users...
            if (!context.Users.Any(u => u.Email == "DemoAdmin@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@Mailinator.com",
                    Email = "DemoAdmin@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                    DisplayName = "The Admin"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            if (!context.Users.Any(u => u.Email == "DemoProjectManager@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoProjectManager@Mailinator.com",
                    Email = "DemoProjectManager@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Project Manager",
                    DisplayName = "The PM"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            if (!context.Users.Any(u => u.Email == "DemoDeveloper@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper@Mailinator.com",
                    Email = "DemoDeveloper@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer",
                    DisplayName = "The Dev"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter@Mailinator.com",
                    Email = "DemoSubmitter@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter",
                    DisplayName = "The Sub"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }
            #endregion

            #region AssignToRoles
            var adminId = userManager.FindByEmail("Sheila.Ward@email.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var submitterId = userManager.FindByEmail("TShorter@mailinator.com").Id;
            userManager.AddToRole(submitterId, "Submitter");

            submitterId = userManager.FindByEmail("Sherri@Mailinator.com").Id;
            userManager.AddToRole(submitterId, "Submitter");

            var PMId = userManager.FindByEmail("DRose@mailinator.com").Id;
            userManager.AddToRole(PMId, "ProjectManager");

            PMId = userManager.FindByEmail("AngieSmith@mailinator.com").Id;
            userManager.AddToRole(PMId, "ProjectManager");

            var DevId = userManager.FindByEmail("CTriplett@mailinator.com").Id;
            userManager.AddToRole(DevId, "Developer");

            DevId = userManager.FindByEmail("AEller@mailinator.com").Id;
            userManager.AddToRole(DevId, "Developer");

            DevId = userManager.FindByEmail("JCooley@mailinator.com").Id;
            userManager.AddToRole(DevId, "Developer");

            DevId = userManager.FindByEmail("LConnelly@mailinator.com").Id;
            userManager.AddToRole(DevId, "Developer");

            DevId = userManager.FindByEmail("SPatel@mailinator.com").Id;
            userManager.AddToRole(DevId, "Developer");

            #endregion

            #region Project Creation
            context.Projects.AddOrUpdate(
                p => p.Name,
                    new Project { Name = "Coder Foundry Blog", Description = "Blog exercise", Created = DateTimeOffset.Now },
                    new Project { Name = "Portfolio Project", Description = "My Portfolio created during Coder Foundry", Created = DateTimeOffset.Now },
                    new Project { Name = "Bug Tracker Project", Description = "System to track tickets in an IT system - can be bugs, or requests for enhancements or documentation", Created = DateTime.Now },
                    new Project { Name = "YelpCamp Project", Description = "Create a new Blog in which multiple people can write posts", Created = DateTime.Now }
            );
            context.SaveChanges();
            #endregion

            //#region Project Assignment
            //var blogProjectId = context.Projects.FirstOrDefault(p => p.Name == "Coder Foundry Blog").Id;
            //var bugTrackerProjectId = context.Projects.FirstOrDefault(p => p.Name == "Bug Tracker Project").Id;
            //var portfolioProject = context.Projects.FirstOrDefault(p => p.Name == "Portfolio Project").Id;
            //var yelpCampProject = context.Projects.FirstOrDefault(p => p.Name == "YelpCamp Project");

            //var projHelper = new ProjectsHelper();
            //#endregion

            #region TicketPriorities
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Name = "None", Description = "Priority has not been determined"},
                    new TicketPriority { Name = "Low", Description = "Lowest priority level" },
                    new TicketPriority { Name = "Medium", Description = "Mid-level priority" },
                    new TicketPriority { Name = "High", Description = "A high priority level requiring quick action" },
                    new TicketPriority { Name = "Urgent", Description = "Highest priority level requiring immediate action" }
            );
            #endregion

            #region TicketStatuses
            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                new TicketStatus { Name = "New/Unassigned", Description = "Has not been approved or has been tabled" },
                new TicketStatus { Name = "Inactive", Description = "Not currently being worked on" },
                new TicketStatus { Name = "Active/Assigned", Description = "Developer is currently working on ticket" },
                new TicketStatus { Name = "Completed", Description = "Development and Testing done, but not deployed" },
                new TicketStatus { Name = "Archived", Description = "Ticket is completed and all work delivered"}
            );
            #endregion

            #region TicketTypes
            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                new TicketType { Name = "Defect", Description = "There is a defect in the application code or logic" },
                new TicketType { Name = "Documentation", Description = "There is a need for documentation/training on the application"},
                new TicketType { Name = "Enhancement", Description = "There is a request for more functionality for the application"}
            );
            #endregion
        }
    }
}
