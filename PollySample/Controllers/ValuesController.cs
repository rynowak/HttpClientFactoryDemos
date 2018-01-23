using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PollySample
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UnreliableClient _client;

        public ValuesController(UnreliableClient client)
        {
            _client = client;
        }
        
        [HttpGet("/")]
        public async Task<ActionResult<Reply>> Index()
        {
            Reply result = null;
            for (var i = 0; i < 30; i++)
            {
                var response = await _client.HttpClient.GetAsync("/unreliable");
                result = await response.Content.ReadAsAsync<Reply>();
            }

            return result;
        }

        public class Reply
        {
            public string Message { get; set; }
        }
    }
}
