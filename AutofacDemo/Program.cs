using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DISample;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddTransient<IHelloWorld, HelloWorld>();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);


//            containerBuilder.RegisterType<HelloWorld>().As<IHelloWorld>();
            
            
            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            var hello = serviceProvider.GetRequiredService<IHelloWorld>();
            var result = hello.SayHello("seamas");
            Console.WriteLine(result);

        }
    }
}