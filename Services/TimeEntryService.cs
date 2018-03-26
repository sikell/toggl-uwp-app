using System;
using System.Threading.Tasks;
using TogglTimer.Services.Api;
using TogglTimer.Services.Api.Model;
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
            return ConvertToTimeEntry(timeEntry);
        }

        public async Task<TimeEntry> StartCurrentTimer(TimeEntry newEntry)
        {
            var newTimeEntry = await _apiClient.StartCurrentTimer(ConvertToTimeEntryDto(newEntry), _authService.GetToken());
            return ConvertToTimeEntry(newTimeEntry);
        }

        public async Task<TimeEntry> StopCurrentTimer()
        {
            var timeEntry = await _apiClient.GetCurrentRunning(_authService.GetToken());
            var newTimeEntry = await _apiClient.StopCurrentTimer(timeEntry.id, _authService.GetToken());
            return ConvertToTimeEntry(newTimeEntry);
        }

        private static TimeEntry ConvertToTimeEntry(TimeEntryDto newTimeEntry)
        {
            if (newTimeEntry == null)
                return null;
            return new TimeEntry()
            {
                Id = newTimeEntry.id,
                Description = newTimeEntry.description,
                Start = DateTime.Parse(newTimeEntry.start)
            };
        }

        private static TimeEntryDto ConvertToTimeEntryDto(TimeEntry newTimeEntry)
        {
            if (newTimeEntry == null)
                return null;
            return new TimeEntryDto()
            {
                id = newTimeEntry.Id,
                description = newTimeEntry.Description,
                start = newTimeEntry.Start.ToString("s")
            };
        }
    }
}