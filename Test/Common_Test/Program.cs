﻿using System;
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

            //var gg = new SRGBColour(0x0C, 0x66, 0xEE).ToHSL();

            var biMap = new BiMap<int, int>()
            {
                { 1, (short)1 },
                { 2, (short)2 },
                { 3, (short)3 },
            };

            biMap[3] = 1;

        }
    }
}
