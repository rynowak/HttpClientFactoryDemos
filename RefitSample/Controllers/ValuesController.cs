using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RefitSample
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IHelloClient _client;

        public ValuesController(IHelloClient client)
        {
            _client = client;
        }
        
        [HttpGet("/")]
        public async Task<ActionResult<Reply>> Index()
        {
            return await _client.GetMessageAsync();
        }
    }
}
