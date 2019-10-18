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
                    AvatarUrl = "/Avatars/AngieSmith.jpg"
                }, "P@ssw0rd");
            }
            if (!context.Users.Any(r => r.UserName == "AEller@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "AEller@mailinator.com",
                    Email = "AEller@mailinator.com",
                    FirstName = "Ares",
                    LastName = "Eller",
                    DisplayName = "AEller",
                    AvatarUrl = "/Avatars/AresEller.jpg"
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
                    AvatarUrl = "/Avatars/DaveRose.jpg"
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
                    AvatarUrl = "/Avatars/Cindy.jpg"
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
                    AvatarUrl = "/Avatars/cooley.jpg"
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
                    AvatarUrl = "/Avatars/LeeConnelly.jpg"
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
                    AvatarUrl = "/Avatars/sarvesh.jpg"
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
                    AvatarUrl = "/Avatars/sheila.jpg"
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
                    AvatarUrl = "/Avatars/SCreech.jpg"
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
                    AvatarUrl = "/Avatars/teresa.jpg"
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
                    DisplayName = "The Admin",
                    AvatarUrl = "/Avatars/kartunix(1).png"
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
                    DisplayName = "The PM",
                    AvatarUrl = "/Avatars/kartunix(2).png"
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
                    DisplayName = "The Dev",
                    AvatarUrl = "/Avatars/kartunix(4).png"
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
                    DisplayName = "The Sub",
                    AvatarUrl = "/Avatars/kartunix(3).png"
                }, WebConfigurationManager.AppSettings["DemoUserPassword"]);
            }
            #endregion

            #region AssignToRoles
            var sheilaId = userManager.FindByEmail("Sheila.Ward@email.com").Id;
            userManager.AddToRole(sheilaId, "Admin");

            var adminId = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var teresaId = userManager.FindByEmail("TShorter@mailinator.com").Id;
            userManager.AddToRole(teresaId, "Submitter");

            var sherriId = userManager.FindByEmail("Sherri@Mailinator.com").Id;
            userManager.AddToRole(sherriId, "Submitter");

            var submitterId = userManager.FindByEmail("DemoSubmitter@Mailinator.com").Id;
            userManager.AddToRole(submitterId, "Submitter");

            var daveId = userManager.FindByEmail("DRose@mailinator.com").Id;
            userManager.AddToRole(daveId, "ProjectManager");

            var angieId = userManager.FindByEmail("AngieSmith@mailinator.com").Id;
            userManager.AddToRole(angieId, "ProjectManager");

            var PMId = userManager.FindByEmail("DemoProjectManager@Mailinator.com").Id;
            userManager.AddToRole(PMId, "ProjectManager");

            var cindyId = userManager.FindByEmail("CTriplett@mailinator.com").Id;
            userManager.AddToRole(cindyId, "Developer");

            var aresId = userManager.FindByEmail("AEller@mailinator.com").Id;
            userManager.AddToRole(aresId, "Developer");

            var cooleyId = userManager.FindByEmail("JCooley@mailinator.com").Id;
            userManager.AddToRole(cooleyId, "Developer");

            var leeId = userManager.FindByEmail("LConnelly@mailinator.com").Id;
            userManager.AddToRole(leeId, "Developer");

            var sarveshId = userManager.FindByEmail("SPatel@mailinator.com").Id;
            userManager.AddToRole(sarveshId, "Developer");

            var DevId = userManager.FindByEmail("DemoDeveloper@mailinator.com").Id;
            userManager.AddToRole(DevId, "Developer");

            #endregion

            #region Project Creation
            DateTime P1Date = new DateTime(2019, 07, 12, 19, 00, 14, 16);
            DateTime P2Date = new DateTime(2019, 06, 23, 17, 06, 25, 32);
            DateTime P3Date = new DateTime(2019, 08, 04, 16, 23, 24, 22);
            DateTime P4Date = new DateTime(2019, 04, 12, 10, 45, 17, 26);
            context.Projects.AddOrUpdate(
                p => p.Name,
                    new Project { Name = "Coder Foundry Blog", Description = "Blog exercise", Created = new DateTimeOffset(P1Date.Year, P1Date.Month, P1Date.Day, P1Date.Hour, P1Date.Minute, P1Date.Second, P1Date.Millisecond, new TimeSpan(2, 0, 0)) },
                    new Project { Name = "Portfolio Project", Description = "My Portfolio created during Coder Foundry", Created = new DateTimeOffset(P2Date.Year, P2Date.Month, P2Date.Day, P2Date.Hour, P2Date.Minute, P2Date.Second, P2Date.Millisecond, new TimeSpan(2, 0, 0)) },
                    new Project { Name = "Bug Tracker Project", Description = "System to track tickets in an IT system - can be bugs, or requests for enhancements or documentation", Created = new DateTimeOffset(P3Date.Year, P3Date.Month, P3Date.Day, P3Date.Hour, P3Date.Minute, P3Date.Second, P3Date.Millisecond, new TimeSpan(2, 0, 0)) },
                    new Project { Name = "YelpCamp Project", Description = "Create a new Blog in which multiple people can write posts", Created = new DateTimeOffset(P4Date.Year, P4Date.Month, P4Date.Day, P4Date.Hour, P4Date.Minute, P4Date.Second, P4Date.Millisecond, new TimeSpan(2, 0, 0)) }
            );
            context.SaveChanges();
            #endregion

            #region Project Assignment
            var blogProjectId = context.Projects.FirstOrDefault(p => p.Name == "Coder Foundry Blog").Id;
            var bugTrackerProjectId = context.Projects.FirstOrDefault(p => p.Name == "Bug Tracker Project").Id;
            var portfolioProjectId = context.Projects.FirstOrDefault(p => p.Name == "Portfolio Project").Id;
            var yelpCampProjectId = context.Projects.FirstOrDefault(p => p.Name == "YelpCamp Project").Id;

            var projHelper = new ProjectsHelper();

            // Assign Sheila to projects
            projHelper.AddUserToProject(sheilaId, portfolioProjectId);
            projHelper.AddUserToProject(sheilaId, blogProjectId);
            projHelper.AddUserToProject(sheilaId, bugTrackerProjectId);
            projHelper.AddUserToProject(sheilaId, yelpCampProjectId);

            // Assign Demo Admin to projects
            projHelper.AddUserToProject(adminId, portfolioProjectId);
            projHelper.AddUserToProject(adminId, blogProjectId);
            projHelper.AddUserToProject(adminId, bugTrackerProjectId);
            projHelper.AddUserToProject(adminId, yelpCampProjectId);

            // Assign Angie to projects
            projHelper.AddUserToProject(angieId, blogProjectId);
            projHelper.AddUserToProject(PMId, portfolioProjectId);

            // Assign Dave to projects
            projHelper.AddUserToProject(daveId, bugTrackerProjectId);
            projHelper.AddUserToProject(daveId, yelpCampProjectId);

            // Assign Demo PM to projects
            projHelper.AddUserToProject(PMId, bugTrackerProjectId);
            projHelper.AddUserToProject(PMId, portfolioProjectId);

            // Assign Teresa to projects
            projHelper.AddUserToProject(teresaId, blogProjectId);
            projHelper.AddUserToProject(teresaId, bugTrackerProjectId);

            // Assign Sherri to projects
            projHelper.AddUserToProject(sherriId, portfolioProjectId);

            // Assign Demo Submitter to projects
            projHelper.AddUserToProject(submitterId, yelpCampProjectId);
            projHelper.AddUserToProject(submitterId, portfolioProjectId);

            // Assign Ares to projects
            projHelper.AddUserToProject(aresId, blogProjectId);
        
            // Assign Sarvesh to projects
            projHelper.AddUserToProject(sarveshId, portfolioProjectId);
            
            // Assign Lee to projects
            projHelper.AddUserToProject(leeId, bugTrackerProjectId);

            // Assign Cindy to projects
            projHelper.AddUserToProject(cindyId, bugTrackerProjectId);

            // Assign Cooley to projects
            projHelper.AddUserToProject(cooleyId, portfolioProjectId);
            projHelper.AddUserToProject(cooleyId, blogProjectId);

            // Assign Demo Developer to projects
            projHelper.AddUserToProject(DevId, bugTrackerProjectId);
            #endregion

            #region Ticket Priorities, Statuses, & Types (require Foreign Keys)
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Name = "None", Description = "Priority has not been determined"},
                    new TicketPriority { Name = "Low", Description = "Lowest priority level" },
                    new TicketPriority { Name = "Medium", Description = "Mid-level priority" },
                    new TicketPriority { Name = "High", Description = "A high priority level requiring quick action" },
                    new TicketPriority { Name = "Urgent", Description = "Highest priority level requiring immediate action" }
            );

            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                new TicketStatus { Name = "New/Unassigned", Description = "Has not been approved or has been tabled" },
                new TicketStatus { Name = "Inactive", Description = "Not currently being worked on" },
                new TicketStatus { Name = "Active/Assigned", Description = "Developer is currently working on ticket" },
                new TicketStatus { Name = "Withdrawn", Description = "Ticket has been withdrawn"},
                new TicketStatus { Name = "Completed", Description = "Development and Testing done, but not deployed" },
                new TicketStatus { Name = "Archived", Description = "Ticket is completed and all work delivered"}
            );

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                new TicketType { Name = "Defect", Description = "There is a defect in the application code or logic" },
                new TicketType { Name = "Documentation", Description = "There is a need for documentation/training on the application"},
                new TicketType { Name = "Enhancement", Description = "There is a request for more functionality for the application"}
            );

            context.SaveChanges();
            #endregion

            #region Ticket creation
            DateTime T1Date = new DateTime(2019, 09, 03, 19, 00, 14, 16);
            DateTime T2Date = new DateTime(2019, 08, 24, 18, 16, 13, 27);
            DateTime T3Date = new DateTime(2019, 09, 09, 19, 00, 14, 16);
            DateTime T4Date = new DateTime(2019, 07, 02, 19, 00, 14, 16);
            DateTime T5Date = new DateTime(2019, 06, 25, 18, 16, 23, 34);
            DateTime T6Date = new DateTime(2019, 08, 19, 20, 08, 37, 52);
            DateTime T7Date = new DateTime(2019, 08, 21, 18, 55, 43, 16);
            context.Tickets.AddOrUpdate(
                p => p.Title,
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Make Icons turn red when active",
                        Description = "Make the Icons in the left navigation turn red when active",
                        PercentComplete = 90,
                        Created = new DateTimeOffset(T1Date.Year, T1Date.Month, T1Date.Day, T1Date.Hour, T1Date.Minute, T1Date.Second, T1Date.Millisecond, new TimeSpan(2, 0, 0)),
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Low").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Add Buttons for Table Downloads",
                        Description = "Add Copy, CSV, Excel, PDF, and Print buttons for tables",
                        PercentComplete = 80,
                        Created = new DateTimeOffset(T2Date.Year, T2Date.Month, T2Date.Day, T2Date.Hour, T2Date.Minute, T2Date.Second, T2Date.Millisecond, new TimeSpan(2, 0, 0)),
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Add Functionality to Edit Projects View",
                        Description = "On Edit Projects, give Admin/PM ability to assign people to projects",
                        PercentComplete = 60,
                        Created = new DateTimeOffset(T3Date.Year, T3Date.Month, T3Date.Day, T3Date.Hour, T3Date.Minute, T3Date.Second, T3Date.Millisecond, new TimeSpan(2, 0, 0)),
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "None").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = portfolioProjectId,
                        OwnerUserId = sherriId,
                        AssignedToUserId = cooleyId,
                        Title = "Activate Google Maps",
                        Description = "The Portfolio Project has a Google Maps area in the 'Contact' section that needs to be activated.",
                        PercentComplete = 50,
                        Created = new DateTimeOffset(T4Date.Year, T4Date.Month, T4Date.Day, T4Date.Hour, T4Date.Minute, T4Date.Second, T4Date.Millisecond, new TimeSpan(2, 0, 0)),
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Urgent").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Active/Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id,
                    },
                    new Ticket
                    {
                        ProjectId = portfolioProjectId,
                        OwnerUserId = sherriId,
                        AssignedToUserId = sarveshId,
                        Title = "Separate JavaScript Exercises",
                        Description = "Bobby wants the Javascript Exercises put in a different section than the Bug Tracker and the Blog.  He also wants them separated out into: Math, Factorial, Fizz-Buzz, and Palindrome.",
                        PercentComplete = 70,
                        Created = new DateTimeOffset(T5Date.Year, T5Date.Month, T5Date.Day, T5Date.Hour, T5Date.Minute, T5Date.Second, T5Date.Millisecond, new TimeSpan(2, 0, 0)),
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "High").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Active/Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        AssignedToUserId = cindyId,
                        Title = "Add List of Users to Project Details",
                        Description = "Add a list of all users on a particular project to that project's Detail page.",
                        PercentComplete = 25,
                        Created = new DateTimeOffset(T6Date.Year, T6Date.Month, T6Date.Day, T6Date.Hour, T6Date.Minute, T6Date.Second, T6Date.Millisecond, new TimeSpan(2, 0, 0)),
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Active/Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        AssignedToUserId = cindyId,
                        Title = "Add List of Tickets to Project Details",
                        Description = "Add a list of all tickets on a particular project to that project's Detail page.",
                        PercentComplete = 45,
                        Created = new DateTimeOffset(T7Date.Year, T7Date.Month, T7Date.Day, T7Date.Hour, T7Date.Minute, T7Date.Second, T7Date.Millisecond, new TimeSpan(2, 0, 0)),
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Active/Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    }
            );
            #endregion
        }
    }
}
