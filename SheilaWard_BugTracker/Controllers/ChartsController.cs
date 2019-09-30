using Microsoft.AspNet.Identity;
using SheilaWard_BugTracker.ChartViewModels;
using SheilaWard_BugTracker.Enumerations;
using SheilaWard_BugTracker.Helpers;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_BugTracker.Controllers
{
    [RequireHttps]
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private TicketDecisionHelper ticketHelper = new TicketDecisionHelper();

        // GET: Charts
        public JsonResult GetHardCodedMorrisBarData()
        {
            var dataSet = new List<MorrisBarChartData>();
            dataSet.Add(new MorrisBarChartData { label = "New/Unassigned", value = 15 });
            dataSet.Add(new MorrisBarChartData { label = "Active/Assigned", value = 70 });
            dataSet.Add(new MorrisBarChartData { label = "Inactive", value = 3 });
            dataSet.Add(new MorrisBarChartData { label = "Completed", value = 200 });
            dataSet.Add(new MorrisBarChartData { label = "Archived", value = 350 });

            return Json(dataSet);
        }

        public JsonResult GetRealMorrisData()
        {
            var dataSet = new List<MorrisBarChartData>();
            foreach (var ticketStatus in db.TicketStatuses.ToList())
            {
                var value = db.TicketStatuses.Find(ticketStatus.Id).Tickets.Count();
                dataSet.Add(new MorrisBarChartData { label = ticketStatus.Name, value = value });
            }
            return Json(dataSet);
        }

        //public JsonResult GetTicketStatusData()
        //{
        //    var data = new TicketStatusData();
        //    foreach (var ticketStatus in db.TicketStatuses.ToList())
        //    {
        //        var value = db.TicketStatuses.Find(ticketStatus.Id).Tickets.Count();
        //        data.Labels.Add(ticketStatus.Name);
        //        data.Values.Add(value);
        //    }
        //    return Json(data);
        //}

        public JsonResult GetTicketStatusData()
        {
            var data = new ChartJSChartData();
            var userTickets = ticketHelper.ListOfUsersTickets();


            foreach (var ticketStatus in userTickets.Select(t => t.TicketStatus).Distinct().ToList()) 
            {
                data.Labels.Add(ticketStatus.Name);
                data.Values.Add(userTickets.Where(t => t.TicketStatus.Name == ticketStatus.Name).Count());
            }
            return Json(data);
        }

        public JsonResult GetTicketPriorityData()
        {
            var data = new ChartJSChartData();
            var userTickets = ticketHelper.ListOfUsersTickets();

            foreach (var ticketPriority in userTickets.Select(t => t.TicketPriority).Distinct().ToList())
            {
                data.Labels.Add(ticketPriority.Name);
                data.Values.Add(userTickets.Where(t => t.TicketPriority.Name == ticketPriority.Name).Count());
            }
            return Json(data);
        }

        public JsonResult GetTicketsPercentDone()
        {
            var data = new ChartJSChartData();
            var userTickets = ticketHelper.ListOfUsersTickets();

            foreach (var ticket in userTickets.Distinct().ToList())
            {
                if (ticket.TicketStatus.Name == "Active/Assigned")
                {
                    var value = ticket.PercentComplete;
                    data.Labels.Add(ticket.Title);
                    data.Values.Add(value);
                }
                
            }

            return Json(data);
        }


        public JsonResult GetTicketsAssignedToDevs()
        {
            var data = new ChartJSGroupedBarData1();

            var mgrId = User.Identity.GetUserId();
            var mgrProjects = projHelper.ListUserProjects(mgrId);
            var devs = new List<ApplicationUser>();
            var developers = new List<ApplicationUser>();
            foreach (var project in mgrProjects.ToList())
            {
                devs.AddRange(projHelper.UsersInRoleOnProject(project.Id, SystemRole.Developer).Distinct());
            }

            developers = devs.Distinct().ToList();

            foreach (var dev in developers.ToList())
            {
                var valActive = db.Tickets.Where(t => t.AssignedToUserId == dev.Id && t.TicketStatus.Name == "Active/Assigned").Count();
                var valComplete = db.Tickets.Where(t => t.AssignedToUserId == dev.Id && t.TicketStatus.Name == "Completed").Count();
                data.Labels.Add(dev.FirstName);
                data.ValueActive.Add(valActive);
                data.ValueComplete.Add(valComplete);
            }
            return Json(data);
        }

        //public JsonResult GetTicketsCompletedByDevs()
        //{
        //    List<string> month = new List<string>();
        //    var month4 = DateTime.Today;
        //    var month3 = DateTime.Today.AddMonths(-1);
        //    var month2 = DateTime.Today.AddMonths(-2);
        //    var month1 = DateTime.Today.AddMonths(-4);
        //    month.Add(month1);
        //    month.Add(month2);
        //    month.Add(month3);
        //    month.Add(month4);

        //    var data = new ChartJSLineChart();

        //    var userTickets = ticketHelper.ListOfUsersTickets();

        //    foreach (var ticket in userTickets.ToList())
        //    {
        //        if (ticket.TicketStatus.Name == "Archived" || ticket.TicketStatus.Name == "Completed")
        //        {
        //            var lastUpdated = ticket.Updated.GetValueOrDefault();
        //            if (lastUpdated.Month)
        //            var valueDateComplete = ticket.TicketStatus.Name;
        //            var valuePriority = ticket.TicketPriority.Name;
        //            var valueType = ticket.TicketType.Name;
        //            data.Labels.Add(month1);
        //            data.ValueStatus.Add(valueStatus);
        //        }

        //    }

        //    return Json(data);
        //}
    }

}