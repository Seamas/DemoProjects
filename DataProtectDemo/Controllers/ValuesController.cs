using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DataProtectDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DPContext _context;
        private IDataProtector _dataProtector;
        public ValuesController(DPContext context, IDataProtectionProvider provider)
        {
            _context = context;
            _dataProtector = provider.CreateProtector("pms");
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            var result = _context.Products.Select(item => new
            {
                ID= this._dataProtector.Protect(item.Id.ToString()), 
                item.Name
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get(string id)
        {
            var originalId = int.Parse(this._dataProtector.Unprotect(id));
            var result = await _context.Products.FindAsync(originalId);

            return Ok(result);
        }
    }
}