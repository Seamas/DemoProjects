using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DISample;

namespace AutofacWcfService
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("net.tcp://127.0.0.1");
            var binding = new NetTcpBinding();

            var type = typeof(HelloWorld);
            var host = new ServiceHost(type, uri);
            host.AddServiceEndpoint(typeof(IHelloWorld), binding, nameof(IHelloWorld));

            host.Open();
            Console.WriteLine("service started");
            Console.ReadLine();
        }
    }
}
