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
            if (!context.Users.Any(r => r.UserName == "AEller@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "AEller@mailinator.com",
                    Email = "AEller@mailinator.com",
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
                    DisplayName = "The Admin",
                    AvatarUrl = "~/Avatars/kartunix(1).png"
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
                    AvatarUrl = "~/Avatars/kartunix(2).png"
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
                    AvatarUrl = "~/Avatars/kartunix(4).png"
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
                    AvatarUrl = "~/Avatars/kartunix(3).png"
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
            context.Projects.AddOrUpdate(
                p => p.Name,
                    new Project { Name = "Coder Foundry Blog", Description = "Blog exercise", Created = DateTimeOffset.Now },
                    new Project { Name = "Portfolio Project", Description = "My Portfolio created during Coder Foundry", Created = DateTimeOffset.Now },
                    new Project { Name = "Bug Tracker Project", Description = "System to track tickets in an IT system - can be bugs, or requests for enhancements or documentation", Created = DateTime.Now },
                    new Project { Name = "YelpCamp Project", Description = "Create a new Blog in which multiple people can write posts", Created = DateTime.Now }
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

            // Assign Angie to projects
            projHelper.AddUserToProject(angieId, blogProjectId);

            // Assign Ares to projects
            projHelper.AddUserToProject(aresId, blogProjectId);

            // Assign Teresa to projects
            projHelper.AddUserToProject(teresaId, blogProjectId);
            projHelper.AddUserToProject(teresaId, bugTrackerProjectId);

            // Assign Sarvesh to projects
            projHelper.AddUserToProject(sarveshId, portfolioProjectId);

            // Assign Sherri to projects
            projHelper.AddUserToProject(sherriId, portfolioProjectId);

            // Assign Dave to projects
            projHelper.AddUserToProject(daveId, bugTrackerProjectId);
            projHelper.AddUserToProject(daveId, yelpCampProjectId);

            // Assign Lee to projects
            projHelper.AddUserToProject(leeId, bugTrackerProjectId);

            // Assign Cindy to projects
            projHelper.AddUserToProject(cindyId, bugTrackerProjectId);

            // Assign Cooley to projects
            projHelper.AddUserToProject(cooleyId, portfolioProjectId);
            projHelper.AddUserToProject(cooleyId, blogProjectId);
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
            context.Tickets.AddOrUpdate(
                p => p.Title,
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Make Icons turn red when active",
                        Description = "Make the Icons in the left navigation turn red when active",
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Add Buttons for Table Downloads",
                        Description = "Add Copy, CSV, Excel, PDF, and Print buttons for tables",
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Add Buttons to Table Header",
                        Description = "Add pop-out, refresh, collapse, and close buttons to table headers.",
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Low").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Add Functionality to Edit Projects View",
                        Description = "On Edit Projects, give Admin/PM ability to assign people to projects",
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
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
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
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
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Active/Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        Title = "Add Buttons for Table Downloads",
                        Description = "Add Copy, CSV, Excel, PDF, and Print buttons for tables",
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        AssignedToUserId = cindyId,
                        Title = "Add List of Users to Project 'Dashboard'",
                        Description = "Add a list of all users on a particular project to that project's 'Dashboard' page.",
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "Active/Assigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    },
                    new Ticket
                    {
                        ProjectId = bugTrackerProjectId,
                        OwnerUserId = teresaId,
                        AssignedToUserId = cindyId,
                        Title = "Add List of Tickets to Project 'Dashboard'",
                        Description = "Add a list of all tickets on a particular project to that project's 'Dashboard' page.",
                        Created = DateTimeOffset.Now,
                        TicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id,
                        TicketStatusId = context.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id,
                        TicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Enhancement").Id,
                    }
            );
            #endregion
        }
    }
}
