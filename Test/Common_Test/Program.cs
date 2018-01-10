using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using HisRoyalRedness.com;

namespace HisRoyalRedness.com.Tests
{
    class Program
    {
        public static void Main()
        {
            //RollableLogWriter writer = new RollableLogWriter("Test.txt");
            //writer.WriteLine("Sync");
            //writer.WriteLineAsync("Async").Wait();

            //var cm = new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9);
            //var v = new ColourVector(1, 2, 3);

            //var srgb = new SRGBColour(12, 34, 56, 78);
            //var hsv = srgb.ToHSV();

            var bCC = new ByteColourComponent(3);
            var uCC = new UnitColourComponent(3);
            var dCC = new DegreeColourComponent(3);
        }
    }
}
