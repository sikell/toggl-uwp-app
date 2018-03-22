namespace toggl_timer.Services.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string ApiToken { get; set; }
    }
}