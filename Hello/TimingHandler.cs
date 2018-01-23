using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Hello
{
    public class TimingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public TimingHandler(ILogger<TimingHandler> logger)
        {
            _logger = logger;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Request started");
            var response = await base.SendAsync(request, cancellationToken);
            _logger.LogInformation("Request took: {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

            return response;
        }
    }
}
