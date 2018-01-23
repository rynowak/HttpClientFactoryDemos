using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient("dns");

            var factory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();

            var client = factory.CreateClient("dns");

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => cts.Cancel();

            while (!cts.IsCancellationRequested)
            {
                var content = await client.GetStringAsync("http://192.168.1.104:5000/");
                Console.WriteLine(content);

                await Task.Delay(300);
            }
        }
    }
}
