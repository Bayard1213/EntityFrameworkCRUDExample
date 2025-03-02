using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechnomediaTestTask.Enums;

namespace TechnomediaTestTask.DTOs.Request
{
    public class CompleteStatusRequestDTO
    {
        public string Note { get; set; }
        public string ResearchNotes { get; set; }
    }
}