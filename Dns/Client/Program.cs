using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client
{
    class Program
    {
        // To try this, run the server project on two machines, and set up https://lock.cmpxchg8b.com/rebinder.html
        // to point to their local IP addresses. You should be able to reach both, but you'll see different output.
        //
        // Example: 
        // - Win10 machine listening on 192.168.1.104:5000
        // - OSX machine listening on 192.168.1.212:5000
        //
        // Setup hosts file entries for these IPs and go nuts. You should be able to change the hosts file and see
        // the results change if this is correct.
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.ConfigureServices(ConfigureServices);
            builder.ConfigureLogging(b =>
            {
                b.AddConsole();
                b.AddFilter((provider, category, level) => 
                {
                    if (category == "Microsoft.Extensions.Http.DefaultHttpClientFactory")
                    {
                        return true;
                    }

                    return false;
                });
                
            });

            var factory = builder.Build().Services.GetRequiredService<IHttpClientFactory>();

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => cts.Cancel();

            var cachedClient = factory.CreateClient("dns");

            while (!cts.IsCancellationRequested)
            {
                var cachedClientContent = await cachedClient.GetStringAsync("http://test.example.lan:5000/");
                Console.WriteLine("Cached: " + DateTime.Now + ": " + cachedClientContent);

                var freshClient = factory.CreateClient("dns");
                var freshClientContent = await freshClient.GetStringAsync("http://test.example.lan:5000/");
                Console.WriteLine("Fresh: " + DateTime.Now + ": " + freshClientContent);
                
                if (cachedClientContent != freshClientContent)
                {
                    Console.WriteLine("DNS CHANGED");
                }

                await Task.Delay(2000);

#if NET462
                ServicePointManager.FindServicePoint(new Uri("http://test.example.lan:5000")).ConnectionLeaseTimeout = 10000;
#endif
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("dns", c =>
            {
            })
            .SetHandlerLifetime(TimeSpan.FromSeconds(10));
        }
    }
}
