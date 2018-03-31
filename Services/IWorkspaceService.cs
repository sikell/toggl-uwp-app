using System.Collections.Immutable;
using System.Threading.Tasks;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
{
    public interface IWorkspaceService
    {
       Task<ImmutableList<Workspace>> ListUserWorkspaces();
    }
}