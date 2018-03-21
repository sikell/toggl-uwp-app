using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using Newtonsoft.Json;
using toggl_timer.Services.Api.Model;

namespace toggl_timer.Services.Api
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

        public async Task<User> GetUser(string apiToken)
        {
            const string requestUri = "me";
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(apiToken + ":api_token"));
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
            return JsonConvert.DeserializeObject<User>(readAsStringAsync);
        }
    }
}