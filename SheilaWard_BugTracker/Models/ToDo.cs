using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_BugTracker.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public string UserId { get; set; }             // Foreign Key - User the ToDo belongs to
        public string Task { get; set; }
        public bool GrayedOut { get; set; }
        public DateTime Due { get; set; }
        public string Color { get; set; }
        public string WordDiff { get; set; }
    }
}