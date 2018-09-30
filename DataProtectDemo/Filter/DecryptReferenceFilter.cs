using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DataProtectDemo.Filter
{
    public class DecryptReferenceFilter: IActionFilter
    {
        private readonly IDataProtector _dataProtector;

        public DecryptReferenceFilter(IDataProtectionProvider provider)
        {
            _dataProtector = provider.CreateProtector("pms").ToTimeLimitedDataProtector();
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.RouteData.Values["id"].ToString();
            Console.WriteLine($"id:{param}");
            var id = int.Parse(this._dataProtector.Unprotect(param));
            context.ActionArguments["id"] = id;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}