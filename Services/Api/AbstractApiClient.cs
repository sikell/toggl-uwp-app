using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MetroLog;
using Newtonsoft.Json;

namespace TogglTimer.Services.Api
{
    public abstract class AbstractApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger _log = LogManagerFactory.DefaultLogManager.GetLogger<AbstractApiClient>();

        protected AbstractApiClient(HttpClient client)
        {
            _client = client;
        }

        protected Task<TResult> DoPut<TResult, TBody>(string url, string credentials, TBody body)
        {
            return DoRequest<TResult, TBody>(HttpMethod.Put, url, credentials, body);
        }

        protected Task<TResult> DoPost<TResult, TBody>(string url, string credentials, TBody body)
        {
            return DoRequest<TResult, TBody>(HttpMethod.Post, url, credentials, body);
        }

        protected Task<T> DoGet<T>(string url, string credentials)
        {
            return DoRequest<T>(HttpMethod.Get, url, credentials);
        }

        protected static string ToBasicAuth(string apiToken)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(apiToken + ":api_token"));
        }

        private Task<TResult> DoRequest<TResult>(HttpMethod httpMethod, string url, string credentials)
        {
            return DoRequest<TResult, object>(httpMethod, url, credentials, null);
        }

        private async Task<TResult> DoRequest<TResult, TBody>(HttpMethod httpMethod, string url, string credentials, TBody body)
        {
            _log.Debug("{0} {1}", httpMethod, url);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress + url),
                Method = httpMethod,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), "Basic " + credentials},
                    {HttpRequestHeader.ContentType.ToString(), "application/json"}
                },
                Content = body != null ? new StringContent(JsonConvert.SerializeObject(body)) : null
            };

            try
            {
                var response = await _client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    _log.Error("{0} {1} failed with {2}! ", httpMethod, url, response.StatusCode);
                    return default(TResult);
                }

                var readAsStringAsync = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(readAsStringAsync);
            }
            catch (HttpRequestException e)
            {
                _log.Error("{0} {1} failed with {2}! ", httpMethod, url, e.Message);
                _log.Error(e.Message, e);
                return default(TResult);
            }
        }
    }
}