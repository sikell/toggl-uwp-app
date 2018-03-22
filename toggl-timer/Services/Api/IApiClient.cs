using System.Threading.Tasks;
using toggl_timer.Services.Api.Model;

namespace toggl_timer.Services.Api
{
    public interface IApiClient
    {
        Task<TimeEntryDto> GetCurrentRunning(string apiToken);
        Task<UserDto> GetUser(string apiToken);
        Task<UserDto> GetUser(string username, string password);
    }
}