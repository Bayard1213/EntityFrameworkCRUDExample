using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnomediaTestTask.DTOs.WorkLog
{
    public class WorkLogDTO
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public string Comments { get; set; }

    }
}