using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toggl_timer.Services.Api.Model;

namespace toggl_timer.Services.Api
{
    public interface IApiClient
    {
        Task<User> GetUser(String apiToken);
    }
}