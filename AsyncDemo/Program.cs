using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            PrintThread();
            Console.WriteLine("--------------");
            ProcessAsync();
            Process();

            Console.ReadLine();
        }


        static async Task<int> ProcessAsync()
        {
            PrintThread();
            await Task.Yield();
            PrintThread();
            return 1;
        }

        static async Task Process()
        {
            PrintThread();
        }


        static void PrintThread()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }
    }
}