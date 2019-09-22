using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.ChartViewModels
{
    public class MorrisBarChartData
    {
        public string label { get; set; }
        public int value { get; set; }
    }

    public class TicketStatusData
    {
        public List<string> Labels = new List<string>();
        public List<int> Values = new List<int>();
    }

    public class TicketsAssignedToDevs
    {
        public List<string> Labels = new List<string>();
        public List<int> Values = new List<int>();
    }
}