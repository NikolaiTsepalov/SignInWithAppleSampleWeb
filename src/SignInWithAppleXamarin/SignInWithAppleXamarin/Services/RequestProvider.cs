using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignInWithAppleXamarin.Services
{
    [Serializable]
    public sealed class RequestProvider : IRequestProvider, IDisposable
    {
        private RequestProvider()
        {
        }
        public static IRequestProvider Instance => Lazy.Value;

        private static readonly Lazy<RequestProvider> Lazy =
            new Lazy<RequestProvider>(() => new RequestProvider(), true);

        public async Task<string> GetAsync(string uri, string token = "")
        {
            try
            {
                var httpClient = CreateOrGetHttpAuthClient(token);

                var response = await httpClient.GetAsync(uri).ConfigureAwait(false);
                if (response != null)
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                throw new Exception($"{nameof(GetAsync)} response is null! uri {uri}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
        private HttpClient _httpAuthClient;
        private HttpClient _httpClient;
        private string _lastToken;
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(400); // TODO 000 40

        private HttpClient CreateOrGetHttpAuthClient(string token)
        {
            if (_httpAuthClient == null || false == token.Equals(_lastToken, StringComparison.Ordinal))
            {
                _lastToken = token;
                _httpAuthClient = new HttpClient();
                _httpAuthClient.Timeout = _timeout;
                _httpAuthClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (false == string.IsNullOrEmpty(token))
                {
                    _httpAuthClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            return _httpAuthClient;
        }

        #region IDisposable Support
        public void Dispose() // call at the App.OnSleep()
        {
            _httpClient?.Dispose();
            _httpClient = null;
            _httpAuthClient?.Dispose();
            _httpAuthClient = null;
        }
        #endregion
    }
}
