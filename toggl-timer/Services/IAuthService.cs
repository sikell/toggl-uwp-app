using System.Threading.Tasks;
using toggl_timer.Services.Api.Model;

namespace toggl_timer.Services
{
    public interface IAuthService
    {
        string GetToken();
        bool IsAuthenticated();
        Task<User> GetUser();
        Task<bool> AuthUser(string username, string password);
    }
}