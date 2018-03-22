using System.Threading.Tasks;
using toggl_timer.Services.Model;

namespace toggl_timer.Services
{
    public interface ITimeEntryService
    {
        Task<TimeEntry> GetCurrent();
    }
}