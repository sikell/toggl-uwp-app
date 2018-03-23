﻿
using System.Threading.Tasks;
using TogglTimer.Services.Model;

namespace TogglTimer.Services
{
    public interface IAuthService
    {
        string GetToken();
        bool IsAuthenticated();
        Task<User> GetUser();
        Task<bool> AuthUser(string username, string password);
    }
}