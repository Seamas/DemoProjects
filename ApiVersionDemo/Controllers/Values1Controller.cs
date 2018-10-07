using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ApiVersionDemo.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("3.0")]
    [Route("api/values")]
//    [Route("api/{v:apiVersion}/values")]
    public class Values1Controller: ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new [] { "hello", "world","version 1.0" };
        }


        [HttpGet, MapToApiVersion("3.0")]
        public IEnumerable<string> GetV3()
        {
            return new [] { "hello", "world","version 3.0" };
        }
    }
}