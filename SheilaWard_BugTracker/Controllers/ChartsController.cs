using SheilaWard_BugTracker.ChartViewModels;
using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_BugTracker.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

        public JsonResult GetRealChartJsData()
        {
            var data = new ChartJsData();
            foreach (var ticketStatus in db.TicketStatuses.ToList())
            {
                var value = db.TicketStatuses.Find(ticketStatus.Id).Tickets.Count();
                data.Labels.Add(ticketStatus.Name);
                data.Values.Add(value);
            }
            return Json(data);
        }
    }

}