using System;
using System.Linq;
using System.Threading.Tasks;
using DataProtectDemo.Attributes;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DataProtectDemo.Controllers
{
    [Route("api/[controller]")]
//    [ApiController]
    public class ProductController : ControllerBase
    {
        private DPContext _context;
        private IDataProtector _dataProtector;
        public ProductController(DPContext context, IDataProtector dataProtector)
        {
            _context = context;
            _dataProtector = dataProtector;
        }
        
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> All()
        {
            var result = _context.Products.Select(item => new
            {
                ID= _dataProtector.Protect(item.Id.ToString()), 
                item.Name
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [DecryptReference("id", typeof(int))]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _context.Products.FindAsync(id);

            return Ok(result);
        }
    }
}