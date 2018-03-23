using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using Newtonsoft.Json;
using TogglTimer.Services.Api.Model;

namespace TogglTimer.Services.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<ApiClient>();

        public ApiClient()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
            };
            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://www.toggl.com/api/v8/")
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TimeEntryDto> GetCurrentRunning(string apiToken)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(apiToken + ":api_token"));
            const string requestUri = "time_entries/current";
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress + requestUri),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), "Basic " + credentials}
                }
            };

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _log.Error("Api access failed! " + response.StatusCode);
                return null;
            }

            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TimeEntryWrapperDto>(readAsStringAsync).data;
        }

        public async Task<UserDto> GetUser(string apiToken)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(apiToken + ":api_token"));
            return await GetUserByBasicAuth(credentials);
        }

        public async Task<UserDto> GetUser(string username, string password)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            return await GetUserByBasicAuth(credentials);
        }

        private async Task<UserDto> GetUserByBasicAuth(string credentials)
        {
            const string requestUri = "me";
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress + requestUri),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), "Basic " + credentials}
                }
            };

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _log.Error("Api access failed! " + response.StatusCode);
                return null;
            }

            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(readAsStringAsync);
        }
    }
}