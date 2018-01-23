using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Hello
{
    public class GithubClient
    {
        public GithubClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        // Put any methods you want here... OR don't, it's up to you.
        public async Task<JObject> GetIndexAsync()
        {
            var response = await HttpClient.GetAsync("/");
            return await response.Content.ReadAsAsync<JObject>();
        }
    }
}
