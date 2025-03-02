using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using TechnomediaTestTask.Enums;

namespace TechnomediaTestTask.DTOs.Request
{
    public class UpdateRequestDTO
    {
        public int ClientId { get; set; }
        public string Notes { get; set; }
        public string ResearchNotes { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RequestStatus Status { get; set; }
    }
}