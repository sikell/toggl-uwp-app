using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using TogglTimer.Services.Api.Model;

namespace TogglTimer.Services.Api
{
    public class ApiClient : AbstractApiClient, IApiClient
    {
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<ApiClient>();

        public ApiClient(HttpClient client) : base(client)
        {
            client.BaseAddress = new Uri("https://www.toggl.com/api/v8/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TimeEntryDto> GetCurrentRunning(string apiToken)
        {
            return (await DoGet<DataWrapperDto<TimeEntryDto>>("time_entries/current", ToBasicAuth(apiToken)))?.data;
        }

        public async Task<UserDto> GetUser(string apiToken)
        {
            return await DoGet<UserDto>("me", ToBasicAuth(apiToken));
        }

        public async Task<UserDto> GetUser(string username, string password)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            return await DoGet<UserDto>("me", credentials);
        }

        public async Task<TimeEntryDto> StartCurrentTimer(NewTimeEntryDto newEntry, string apiToken)
        {
            var entry = new TimeEntrySingleWrapperDto<NewTimeEntryDto> {time_entry = newEntry};
            return (await DoPost<DataWrapperDto<TimeEntryDto>, TimeEntrySingleWrapperDto<NewTimeEntryDto>>(
                "time_entries/start", ToBasicAuth(apiToken), entry))?.data;
        }

        public async Task<TimeEntryDto> StopCurrentTimer(long entryId, string apiToken)
        {
            return (await DoPut<DataWrapperDto<TimeEntryDto>, TimeEntryDto>(
                "time_entries/" + entryId + "/stop", ToBasicAuth(apiToken), null))?.data;
        }

        public async Task<Collection<WorkspaceDto>> ListWorkspaces(string apiToken)
        {
            return await DoGet<Collection<WorkspaceDto>>("workspaces", ToBasicAuth(apiToken));
        }

        public async Task<Collection<ProjectDto>> ListProjects(long workspaceId, string apiToken)
        {
            return await DoGet<Collection<ProjectDto>>("workspaces/" + workspaceId + "/projects", ToBasicAuth(apiToken));
        }

        public async Task<ProjectDto> GetProject(long projectId, string apiToken)
        {
            return (await DoGet<DataWrapperDto<ProjectDto>>("projects/" + projectId, ToBasicAuth(apiToken))).data;
        }

    }
}