using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using TechnomediaTestTask.Enums;

namespace TechnomediaTestTask.DTOs.Report
{
    public class ReportDTO
    {
        public int TeamId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Month SelectedMonth { get; set; }
        public string TeamName { get; set; }
        public int CompletedRequests { get; set; }
        public int TotalTimeSpent { get; set; }
    }
}