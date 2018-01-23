using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Hello
{
    public class CacheHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private byte[] _cached;

        public CacheHandler(ILogger<CacheHandler> logger)
        {
            _logger = logger;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var bytes = await new HttpMessageContent(response).ReadAsByteArrayAsync();
                    _cached = bytes;

                    return await DeserializeAsync(bytes);
                }
                else if (_cached != null)
                {
                    _logger.LogWarning("Using cached response");
                    return await DeserializeAsync(_cached);
                }

                return response;
            }
            catch when (_cached != null)
            {
                _logger.LogWarning("Using cached response");
                return await DeserializeAsync(_cached);
            }
        }

        private static async Task<HttpResponseMessage> DeserializeAsync(byte[] bytes)
        {
            var content = new ByteArrayContent(bytes);
            content.Headers.Add("Content-Type", "application/http; msgtype=response");
            return await content.ReadAsHttpResponseMessageAsync();
        }
    }

}
