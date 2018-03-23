using System;

namespace TogglTimer.Services.Model
{
    public class TimeEntry
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime At { get; set; }
    }
}