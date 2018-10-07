using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ApiVersionDemo.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/values")]
//    [Route("api/{v:apiVersion}/values")]
    public class Values2Controller: ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new [] { "hello", "world","version 2.0" };
        }

    }
}