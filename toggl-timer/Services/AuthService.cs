using System.Threading.Tasks;
using MetroLog;
using toggl_timer.Services.Api;
using toggl_timer.Services.Api.Model;

namespace toggl_timer.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApiClient _apiClient;
        private string _token = null;
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<AuthService>();

        public AuthService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public string GetToken()
        {
            return _token;
        }

        public bool IsAuthenticated()
        {
            // TODO test, if token is valid
            return _token != null;
        }

        public Task<UserDto> GetUser()
        {
            return _apiClient.GetUser(_token);
        }

        public async Task<bool> AuthUser(string username, string password)
        {
            var user = await _apiClient.GetUser(username, password);
            if (user == null)
            {
                _log.Error("Can not access api.");
                return false;
            }
            _token = user.data.api_token;
            return true;
        }
    }
}