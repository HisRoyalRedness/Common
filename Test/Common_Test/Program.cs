using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HisRoyalRedness.com.Tests
{
    class Program
    {
        public static void Main()
        {
            //RollableLogWriter writer = new RollableLogWriter("Test.txt");
            //writer.WriteLine("Sync");
            //writer.WriteLineAsync("Async").Wait();

            var srgb = new SRGBColour(12, 34, 56, 78);
            var hsv = srgb.ToHSV();

        }
    }
}
