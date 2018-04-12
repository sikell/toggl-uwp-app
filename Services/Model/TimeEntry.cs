using System;
using TogglTimer.Services.Api.Model;

namespace TogglTimer.Services.Model
{
    public class TimeEntry
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public Project Project { get; set; }
        public TimeSpan Duration { get; set; }
    }
}