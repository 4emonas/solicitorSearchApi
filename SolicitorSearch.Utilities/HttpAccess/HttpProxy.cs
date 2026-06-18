using SolicitorSearch.Utilities.HttpAccess.Interfaces;
using System.Net;

namespace SolicitorSearch.Utilities.HttpAccess
{
    public class HttpProxy : IHttpProxy
    {
        private static readonly HttpClient _httpClient;

        static HttpProxy()
        {
            var handler = new SocketsHttpHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                MaxConnectionsPerServer = Math.Max(64, Environment.ProcessorCount * 32),
                AllowAutoRedirect = true
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30),
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };
        }

        public async Task<string> GetHtmlResponseAsync(string url)
        {
            return await GetHtmlResponseAsync(url, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task<string> GetHtmlResponseAsync(string url, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL must be provided", nameof(url));
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Clear();
            request.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9");

            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return content;
        }
    }
}
