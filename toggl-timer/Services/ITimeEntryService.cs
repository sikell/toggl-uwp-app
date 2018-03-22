using System.Threading.Tasks;
using toggl_timer.Services.Api.Model;

namespace toggl_timer.Services
{
    public interface ITimeEntryService
    {
        Task<TimeEntry> GetCurrent();
    }
}