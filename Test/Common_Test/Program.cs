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


            var r = new RGBColourComponent();
            r.Value = 127;

            byte u = r;

            var val = (byte)58;
            r = (RGBColourComponent)val;

            var srgb = new SRGBColour(12, 34, 56, 78);

            var cc = (Color)srgb;
            var ccs = cc.ToString();

            var hsv = new HSVColour(1.0 / 3.0, 2.0 / 3.0, .5);

            srgb.ToHSV();
        }
    }
}
