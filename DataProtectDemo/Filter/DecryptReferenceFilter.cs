using System;
using System.Data.Common;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DataProtectDemo.Filter
{
    public class DecryptReferenceFilter: IActionFilter
    {
        private readonly IDataProtector _dataProtector;
        private readonly string _keyName;
        private readonly Type _type;
        
        public DecryptReferenceFilter(IDataProtector dataProtector, string keyName, Type type)
        {
            _dataProtector = dataProtector;
            _keyName = keyName;
            _type = type;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.RouteData.Values[_keyName].ToString();
            var id = Convert.ChangeType(_dataProtector.Unprotect(param), _type) ;
            context.ActionArguments[_keyName] = id;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
//            Console.WriteLine(context.Result.ToString());
        }
    }
}