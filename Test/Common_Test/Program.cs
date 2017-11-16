using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisRoyalRedness.com.Tests
{
    class Program
    {
        public static void Main()
        {
            RollableLogWriter writer = new RollableLogWriter("Test.txt");
            writer.WriteLine("Sync");
            writer.WriteLineAsync("Async").Wait();
        }
    }
}
