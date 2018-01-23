
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ExampleServer.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET api/values
        [HttpGet("/helloworld")]
        public ActionResult<object> Hello()
        {
            return new { message = "Hi there!", };
        }

        public static volatile int Counter;

        [HttpGet("/unreliable")]
        public async Task<ActionResult<object>> Unreliable()
        {
            if (Interlocked.Increment(ref Counter) % 2 == 0)
            {
                await Task.Delay(5000);
                return new ObjectResult(new { message = "Sorry can't help ya.", }) { StatusCode = 500 };
            }

            return new { message = "Here you go, I had one ready!", };
        }
    }
}
