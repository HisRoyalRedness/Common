using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace fletcher.org
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Shutdown += (o, e) => Console.WriteLine("ShutDown");

            new Program().Start();

        }

        void Start()
        {
            Console.WriteLine("Hit Enter to quit");
            //Console.ReadLine();
            Console.WriteLine("Clean exit");
            CommandLine.Wait();

        }

    }
}
