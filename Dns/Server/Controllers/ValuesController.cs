using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("/")]
        public ActionResult<object> Get()
        {
            return new { message = $"Hello from, {Environment.MachineName}", };
        }
    }
}
