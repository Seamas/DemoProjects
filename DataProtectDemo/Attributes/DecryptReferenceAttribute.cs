using System;
using DataProtectDemo.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DataProtectDemo.Attributes
{
    public class DecryptReferenceAttribute: TypeFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="type">类型名称，默认是String</param>
        public DecryptReferenceAttribute(string columnName, Type type = null) 
            : base(typeof(DecryptReferenceFilter))
        {
            Arguments = new object[] { columnName, type ?? typeof(string) };
        }

    }
}