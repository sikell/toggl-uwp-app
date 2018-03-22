using System.Threading.Tasks;
using toggl_timer.Services.Api;
using toggl_timer.Services.Api.Model;

namespace toggl_timer.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private readonly IAuthService _authService;
        private readonly IApiClient _apiClient;

        public TimeEntryService(IAuthService authService, IApiClient apiClient)
        {
            _authService = authService;
            _apiClient = apiClient;
        }

        public Task<TimeEntry> GetCurrent()
        {
            return _apiClient.GetCurrentRunning(_authService.GetToken());
        }
    }
}