using System;
using System.Linq;
using System.Threading.Tasks;
using DataProtectDemo.Attributes;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DataProtectDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private DPContext _context;
        private ITimeLimitedDataProtector _dataProtector;
        public ProductController(DPContext context, IDataProtectionProvider provider)
        {
            _context = context;
            _dataProtector = provider.CreateProtector("pms").ToTimeLimitedDataProtector();
        }
        
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            var result = _context.Products.Select(item => new
            {
                ID= this._dataProtector.Protect(item.Id.ToString(), TimeSpan.FromMinutes(10)), 
                item.Name
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        [DecryptReference]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _context.Products.FindAsync(id);

            return Ok(result);
        }
    }
}