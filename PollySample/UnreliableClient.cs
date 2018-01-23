using System.Net.Http;

namespace PollySample
{
    public class UnreliableClient
    {
        public UnreliableClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }
    }
}