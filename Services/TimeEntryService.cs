using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TogglTimer.Services.Api;
using TogglTimer.Services.Api.Model;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
{
    public class TimeEntryService : ITimeEntryService
    {
        private const string CreatedWith = "toggle_uwp_client";

        private readonly IAuthService _authService;
        private readonly IApiClient _apiClient;
        private readonly IProjectService _projectService;

        public TimeEntryService(IAuthService authService, IApiClient apiClient, IProjectService projectService)
        {
            _authService = authService;
            _apiClient = apiClient;
            _projectService = projectService;
        }

        public async Task<TimeEntry> GetCurrent()
        {
            var timeEntry = await _apiClient.GetCurrentRunning(_authService.GetToken());
            return await ConvertToTimeEntry(timeEntry);
        }

        public async Task<TimeEntry> StartCurrentTimer(TimeEntry newEntry)
        {
            var newTimeEntry = await _apiClient.StartCurrentTimer(ConvertToTimeEntryDto(newEntry), _authService.GetToken());
            return await ConvertToTimeEntry(newTimeEntry);
        }

        public async Task<TimeEntry> StopCurrentTimer()
        {
            var timeEntry = await _apiClient.GetCurrentRunning(_authService.GetToken());
            var newTimeEntry = await _apiClient.StopCurrentTimer(timeEntry.id, _authService.GetToken());
            return await ConvertToTimeEntry(newTimeEntry);
        }

        public async Task<ImmutableList<TimeEntry>> ListLastEntries()
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Now;
            var timeEntries = await _apiClient.ListTimeEntries(startDate, endDate, _authService.GetToken());
            return (await Task.WhenAll(timeEntries.Select(ConvertToTimeEntry))).ToImmutableList();
        }

        private async Task<TimeEntry> ConvertToTimeEntry(TimeEntryDto newTimeEntry)
        {
            if (newTimeEntry == null)
                return null;
            return new TimeEntry()
            {
                Id = newTimeEntry.id,
                Description = newTimeEntry.description,
                Start = DateTime.Parse(newTimeEntry.start),
                Project = newTimeEntry.pid == null ? null : await _projectService.GetProject(newTimeEntry.pid.Value)
            };
        }

        private static NewTimeEntryDto ConvertToTimeEntryDto(TimeEntry newTimeEntry)
        {
            if (newTimeEntry == null)
                return null;
            return new NewTimeEntryDto()
            {
                id = newTimeEntry.Id,
                description = newTimeEntry.Description,
                start = newTimeEntry.Start.ToString("s"),
                pid = newTimeEntry.Project?.Id,
                duration = -1,
                created_with = CreatedWith
            };
        }
    }
}