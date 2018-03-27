using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TogglTimer.Services.Api;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IApiClient _apiClient;
        private readonly IAuthService _authService;

        public WorkspaceService(IApiClient apiClient, IAuthService authService)
        {
            _apiClient = apiClient;
            _authService = authService;
        }

        public async Task<ImmutableList<Workspace>> ListUserWorkspaces()
        {
            return (await _apiClient.ListWorkspaces(_authService.GetToken())).Select(s => new Workspace()
            {
                Id = s.id,
                Name = s.name
            }).ToImmutableList();
        }
    }
}