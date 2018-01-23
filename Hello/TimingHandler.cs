using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hello
{
    public class TimingHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Request started");
            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine("Request took: {0}ms", stopwatch.ElapsedMilliseconds);

            return response;
        }
    }
}
