using System.Threading.Tasks;
using TogglTimer.Services.Api.Model;

namespace TogglTimer.Services.Api
{
    public interface IApiClient
    {
        Task<TimeEntryDto> GetCurrentRunning(string apiToken);
        Task<UserDto> GetUser(string apiToken);
        Task<UserDto> GetUser(string username, string password);
    }
}