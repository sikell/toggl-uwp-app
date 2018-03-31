using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TogglTimer.Services.Api.Model;
using TogglTimer.Services.Model;

namespace TogglTimer.Services.Api
{
    public class ProjectService : IProjectService
    {
        private readonly IApiClient _apiClient;
        private readonly IAuthService _authService;

        public ProjectService(IApiClient apiClient, IAuthService authService)
        {
            _apiClient = apiClient;
            _authService = authService;
        }

        public async Task<ImmutableList<Project>> ListWorkspaceProjects(Workspace workspace)
        {
            if (workspace == null) return null;
            return (await _apiClient.ListProjects(workspace.Id, _authService.GetToken())).Select(p => new Project()
            {
                Id = p.id,
                Active = p.active,
                Name = p.name
            }).ToImmutableList();
        }

        public async Task<Project> GetProject(long projectId)
        {
            var project = await _apiClient.GetProject(projectId, _authService.GetToken());
            return new Project()
            {
                Id = project.id,
                Active = project.active,
                Name = project.name
            };
        }
    }
}