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

    public class ChartJSChartData
    {
        public List<string> Labels = new List<string>();
        public List<int> Values = new List<int>();
    }

    public class ChartJSGroupedBarData1
    {
        public List<string> Labels = new List<string>();
        public List<int> ValueActive = new List<int>();
        public List<int> ValueComplete = new List<int>();
    }

    public class ChartJSLineChart
    {
        public List<string> Labels = new List<string>();
        public List<int> ValueStatus = new List<int>();
        public List<int> ValueComplete = new List<int>();
    }
}