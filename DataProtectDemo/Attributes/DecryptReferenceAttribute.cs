using System;
using DataProtectDemo.Filter;
using Microsoft.AspNetCore.Mvc;

namespace DataProtectDemo.Attributes
{
    public class DecryptReferenceAttribute: TypeFilterAttribute
    {
        public DecryptReferenceAttribute() : base(typeof(DecryptReferenceFilter))
        {
        }
    }
}