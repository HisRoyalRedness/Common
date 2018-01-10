//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FluentAssertions;
//using FluentAssertions.Primitives;
//using FluentAssertions.Execution;

//namespace HisRoyalRedness.com.Tests
//{
//#if COLOUR_SINGLE
//    using ColourPrimitive = Single;
//#else
//    using ColourPrimitive = Double;
//#endif

//    [TestClass]
//    public class ColourConversion_Test
//    {
//        [TestMethod]
//        public void Test_RGB_to_HSV()
//        {
//            // Extremes
//            new SRGBColour(0x00, 0x00, 0x00).ToHSV().Should().Be(new HSVColour(0, 0, 0, 1));        // Black
//            new SRGBColour(0xff, 0xff, 0xff).ToHSV().Should().Be(new HSVColour(0, 0, 1, 1));        // White
//            new SRGBColour(0xff, 0x00, 0x00).ToHSV().Should().Be(new HSVColour(0, 1, 1, 1));        // Red
//            new SRGBColour(0x00, 0xff, 0x00).ToHSV().Should().Be(new HSVColour(120, 1, 1, 1));      // Green
//            new SRGBColour(0x00, 0x00, 0xff).ToHSV().Should().Be(new HSVColour(240, 1, 1, 1));      // Blue

//            // Arbitrary colours
//            new SRGBColour(0x0C, 0x66, 0xEE).ToHSV().Should().BeApproximately(new HSVColour(216.106, 0.94957, 0.93333), 0.001);
//            new SRGBColour(0x38, 0x08, 0x68).ToHSV().Should().BeApproximately(new HSVColour(270.000, 0.92308, 0.40784), 0.001);
//            new SRGBColour(0x38, 0xC9, 0x5F).ToHSV().Should().BeApproximately(new HSVColour(136.138, 0.72140, 0.78824), 0.001);
//            new SRGBColour(0xA8, 0x34, 0x5B).ToHSV().Should().BeApproximately(new HSVColour(339.828, 0.69047, 0.65882), 0.001);
//        }
//    }
//}
