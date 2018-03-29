namespace TogglTimer.Services.Api.Model
{
    public class NewTimeEntryDto
    {
        public long id { get; set; }
        public long? pid { get; set; }
        public string start { get; set; }
        public long duration = -1;
        public string description { get; set; }
        public string created_with { get; set; }
    }
}