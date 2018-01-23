using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Hello.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _factory;

        public HomeController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Example0");
        }

        #region Example0

        // It's easy to create a client and use it.
        // 
        // WebAPI.Client is back! (ReadAsAsync<>)
        //
        // 5.2.4 release coming soon, support for netstandard2.0 without gross workarounds.
        public async Task<Reply> Example0()
        {
            var client = _factory.CreateClient("example");
            var response = await client.GetAsync("http://localhost:5000/helloworld");
            var result = await response.Content.ReadAsAsync<Reply>();
            return result;
        }

        public class Reply
        {
            public string Message { get; set; }
        }

        #endregion

        #region Example_1

        // A more complex client can configure per-client settings
        public async Task<string> Example1()
        {
            var client = _factory.CreateClient("github");
            var result = await client.GetStringAsync("/");
            return result;
        }

        #endregion

        #region Example_2

        // The 'typed client' is now available from the service provider
        public async Task<JObject> Example2([FromServices]GithubClient client) 
        {
            return await client.GetIndexAsync();
        }

        #endregion

        #region Example_3

        // Let's add a custom handler
        public async Task<string> Example3()
        {
            var output = new StringBuilder();

            var client = _factory.CreateClient("unreliable");

            for (var i = 0; i < 5; i++)
            {
                // Sometimes our server will return a 500
                try
                {
                    var result = await client.GetStringAsync("/unreliable");
                    output.Append(result);
                }
                catch { }
            }

            return output.ToString();
        }

        #endregion
    }
}
