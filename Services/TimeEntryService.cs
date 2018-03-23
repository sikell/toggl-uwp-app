using System;
using System.Threading.Tasks;
using TogglTimer.Services.Api;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
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

        public async Task<TimeEntry> GetCurrent()
        {
            var timeEntry = await _apiClient.GetCurrentRunning(_authService.GetToken());
            return new TimeEntry()
            {
                Id = timeEntry.id,
                Description = timeEntry.description,
                At = DateTime.Parse(timeEntry.at)
            };
        }
    }
}