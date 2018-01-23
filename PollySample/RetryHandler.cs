using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;

namespace PollySample
{
    public class RetryHandler : DelegatingHandler
    {
        private readonly ILogger<RetryHandler> _logger;

        public RetryHandler(ILogger<RetryHandler> logger)
        {
            _logger = logger;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, (n) => TimeSpan.FromSeconds(n), (Action<Exception, TimeSpan, int, Context>)LogFailure)
                .ExecuteAsync(async () =>
                {
                    var response = await base.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    return response;
                })
            ;
        }

        private void LogFailure(Exception exception, TimeSpan waitTime, int retryCount, Context context)
        {
            _logger.LogWarning("Retrying again after {WaitTime} - count: {RetryCount}", waitTime, retryCount);
        }
    }
}
