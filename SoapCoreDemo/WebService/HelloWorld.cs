using System;
using System.IO;
using System.ServiceModel;
using Microsoft.AspNetCore.Hosting;

namespace SoapCoreDemo.WebService
{
    public class HelloWorld: IHelloWorld
    {
        private IHostingEnvironment _environment;

        public HelloWorld(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        
        public string SayHello(string name)
        {
            return "hello " + name;
        }

        public byte[] Download(string name)
        {
            var basePath = _environment.ContentRootPath;
            var path = Path.Combine(basePath, "download", name);
            return File.ReadAllBytes(path);
        }
    }
}