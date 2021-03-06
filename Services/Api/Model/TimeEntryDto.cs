﻿namespace TogglTimer.Services.Api.Model
{
    public class TimeEntryDto
    {
        public long id { get; set; }
        public string gui { get; set; }
        public long wid { get; set; }
        public long? pid { get; set; }
        public bool billable { get; set; }
        public string start { get; set; }
        public long duration { get; set; }
        public string description { get; set; }
        public bool duronly { get; set; }
        public string at { get; set; }
        public long uid { get; set; }
    }
}