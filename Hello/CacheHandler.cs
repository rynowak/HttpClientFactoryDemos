using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hello
{
    public class CacheHandler : DelegatingHandler
    {
        private byte[] _cached;

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Response was successful");

                    var bytes = await new HttpMessageContent(response).ReadAsByteArrayAsync();
                    _cached = bytes;

                    return await DeserializeAsync(bytes);
                }
                else if (_cached != null)
                {
                    Console.WriteLine("Using cached response");
                    return await DeserializeAsync(_cached);
                }

                return response;
            }
            catch when (_cached != null)
            {
                Console.WriteLine("Using cached response");
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
