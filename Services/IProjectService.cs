using System.Collections.Immutable;
using System.Threading.Tasks;
using TogglTimer.Services.Api.Model;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
{
    public interface IProjectService
    {
        Task<ImmutableList<Project>> ListWorkspaceProjects(Workspace workspace);
        Task<Project> GetProject(long projectId);
    }
}