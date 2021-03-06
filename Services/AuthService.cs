﻿using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using MetroLog;
using TogglTimer.Services.Api;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
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
            if (IsAuthenticated()) return _token;
            _log.Error("User not authenticated!");
            return null;
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
                return false;

            _log.Info("Stored user credentials were found for {0}", credential.UserName);
            credential.RetrievePassword();
            _token = credential.Password;
            return true;
        }

        public async Task<User> GetUser()
        {
            if (!IsAuthenticated())
            {
                _log.Error("User not authenticated!");
                return null;
            }

            var user = await _apiClient.GetUser(_token);
            return (user == null) ? null : new User()
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

        public bool LogoutUser()
        {
            _token = null;
            var vault = new PasswordVault();
            var credentialFromLocker = GetCredentialFromLocker();
            if (credentialFromLocker != null)
                vault.Remove(credentialFromLocker);
            return true;
        }

        private PasswordCredential GetCredentialFromLocker()
        {
            var vault = new PasswordVault();
            try
            {
                var credentialList = vault.FindAllByResource(ResourceName);

                if (credentialList.Count <= 0)
                {
                    _log.Info("No stored credentials found.");
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
            }
            catch (COMException e)
            {
                _log.Info(e.Message, e);
            }

            return null;
        }
    }
}