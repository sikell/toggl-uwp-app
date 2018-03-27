using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TogglTimer.Services.Api.Model;

namespace TogglTimer.Services.Api
{
    public interface IApiClient
    {
        Task<TimeEntryDto> GetCurrentRunning(string apiToken);
        Task<UserDto> GetUser(string apiToken);
        Task<UserDto> GetUser(string username, string password);
        Task<TimeEntryDto> StartCurrentTimer(TimeEntryDto newEntry, string apiToken);
        Task<TimeEntryDto> StopCurrentTimer(long entryId, string apiToken);
        Task<Collection<WorkspaceDto>> ListWorkspaces(string apiToken);
        Task<Collection<ProjectDto>> ListProjects(long workspaceId, string apiToken);
        Task<ProjectDto> GetProject(long projectId, string apiToken);
    }
}