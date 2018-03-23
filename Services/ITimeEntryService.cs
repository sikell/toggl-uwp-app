using System.Threading.Tasks;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
{
    public interface ITimeEntryService
    {
        Task<TimeEntry> GetCurrent();
    }
}