using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Wcf;
using DISample;

namespace AutofactWcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            #region 动态注册

            var type = typeof(ChannelFactory<>).MakeGenericType(typeof(IHelloWorld));
            var singleton = Activator.CreateInstance(type, new object[] { new NetTcpBinding(), "net.tcp://127.0.0.1/IHelloWorld" });

            builder.Register(c => singleton)
                .As(type)   //使用动态注册,必须增加 As(), 否则 singleton会被当作 object类型注册, 导致后面获取不到类型
                .SingleInstance();

            #endregion


            #region 静态注册

            //var singleton = new ChannelFactory<IHelloWorld>
            //    (new NetTcpBinding(), "net.tcp://127.0.0.1/IHelloWorld");
            //builder.Register(c => singleton)
            //    .SingleInstance();

            #endregion

            builder.Register(c => c.Resolve<ChannelFactory<IHelloWorld>>().CreateChannel())
                .As<IHelloWorld>()
                .UseWcfSafeRelease();


            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var hello = scope.Resolve<IHelloWorld>();
                var result = hello.SayHello("world");
                Console.WriteLine(result);
            }
        }
    }
}
