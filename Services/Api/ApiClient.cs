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
            return (await DoGet<TimeEntryWrapperDto>("time_entries/current", ToBasicAuth(apiToken))).data;
        }

        public async Task<UserDto> GetUser(string apiToken)
        {
            return await DoGet<UserDto>("me", ToBasicAuth(apiToken));
        }

        public async Task<UserDto> GetUser(string username, string password)
        {
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
            return await DoGet<UserDto>("me", credentials);
        }

        public async Task<TimeEntryDto> StartCurrentTimer(TimeEntryDto newEntry, string apiToken)
        {
            return await DoPost<TimeEntryDto, TimeEntryDto>("time_entries/start", ToBasicAuth(apiToken), newEntry);
        }

        public async Task<TimeEntryDto> StopCurrentTimer(string apiToken)
        {
            return await DoPost<TimeEntryDto, TimeEntryDto>("time_entries/stop", ToBasicAuth(apiToken), null);
        }

        private async Task<TResult> DoPost<TResult, TBody>(string url, string credentials, TBody body)
        {
            _log.Debug("POST {0}", url);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress + url),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), "Basic " + credentials},
                    {HttpRequestHeader.ContentType.ToString(), "application/json"}
                },
                Content = body != null ? new StringContent(JsonConvert.SerializeObject(body)) : null
            };

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _log.Error("POST {0} failed with {1}! ", url, response.StatusCode);
                return default(TResult);
            }

            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(readAsStringAsync);
        }

        private async Task<T> DoGet<T>(string url, string credentials)
        {
            _log.Debug("GET {0}", url);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress + url),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), "Basic " + credentials}
                }
            };

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _log.Error("GET {0} failed with {1}! ", url, response.StatusCode);
                return default(T);
            }

            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(readAsStringAsync);
        }

        private static string ToBasicAuth(string apiToken)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(apiToken + ":api_token"));
        }
    }
}