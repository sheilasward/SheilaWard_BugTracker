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

    public class ChartJsData
    {
        public List<string> Labels = new List<string>();
        public List<int> Values = new List<int>();
    }
}