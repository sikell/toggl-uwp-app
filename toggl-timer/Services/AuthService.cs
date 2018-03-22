using System.Threading.Tasks;
using Windows.Security.Credentials;
using MetroLog;
using toggl_timer.Services.Api;
using toggl_timer.Services.Model;

namespace toggl_timer.Services
{
    public class AuthService : IAuthService
    {
        private const string ResourceName = "ToggleApi";
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
            _log.Info("Start authenticate user");
            if (_token != null)
            {
                // TODO test, if token is valid
                _log.Info("Token is valid {0}", _token);
                return true;
            }

            _log.Info("Try to load credentials");
            var credential = GetCredentialFromLocker();
            if (credential == null)
            {
                _log.Info("No credentials found ");
                return false;
            }

            _log.Info("Stored user credentials were found for {0}", credential.UserName);
            _token = credential.Password;
            return true;
        }

        public async Task<User> GetUser()
        {
            var user = await _apiClient.GetUser(_token);
            return new User()
            {
                Id = user.data.id,
                Email = user.data.email,
                Fullname = user.data.fullname,
                ApiToken = user.data.api_token
            };
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
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(ResourceName, user.data.email, _token));
            return true;
        }

        private PasswordCredential GetCredentialFromLocker()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(ResourceName);
            if (credentialList.Count <= 0)
            {
                return null;
            }

            if (credentialList.Count == 1)
            {
                return credentialList[0];
            }

            _log.Error("There are multiple usernames stored! Remove all!");
            foreach (var c in credentialList)
            {
                vault.Remove(c);
            }

            return null;
        }
    }
}