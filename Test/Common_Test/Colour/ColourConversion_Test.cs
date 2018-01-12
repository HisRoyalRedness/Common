using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;

namespace HisRoyalRedness.com.Tests
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    [TestClass]
    public class ColourConversion_Test
    {
        [TestMethod]
        [TestCategory(nameof(HSVColour))]
        [TestCategory(nameof(SRGBColour))]
        public void Test_RGB_to_HSV()
        {
            // Extremes
            new SRGBColour(0x00, 0x00, 0x00).ToHSV().Should().Be(new HSVColour(  0, 0, 0, 1), "black");
            new SRGBColour(0xff, 0xff, 0xff).ToHSV().Should().Be(new HSVColour(  0, 0, 1, 1), "white");
            new SRGBColour(0xff, 0x00, 0x00).ToHSV().Should().Be(new HSVColour(  0, 1, 1, 1), "red");
            new SRGBColour(0x00, 0xff, 0x00).ToHSV().Should().Be(new HSVColour(120, 1, 1, 1), "green");
            new SRGBColour(0x00, 0x00, 0xff).ToHSV().Should().Be(new HSVColour(240, 1, 1, 1), "blue");

            // Alpha should pass through
            new SRGBColour(0x00, 0x00, 0x00, 79).ToHSV().Should().Be(new HSVColour(0, 0, 0, 79.0/255.0), "black with alpha");

            // Arbitrary colours
            // Test values taken from http://www.easyrgb.com/en/convert.php
            new SRGBColour(0x0C, 0x66, 0xEE).ToHSV().Should().BeApproximately(new HSVColour(216.106, 0.94957, 0.93333), 0.001);
            new SRGBColour(0x38, 0x08, 0x68).ToHSV().Should().BeApproximately(new HSVColour(270.000, 0.92308, 0.40784), 0.001);
            new SRGBColour(0x38, 0xC9, 0x5F).ToHSV().Should().BeApproximately(new HSVColour(136.138, 0.72140, 0.78824), 0.001);
            new SRGBColour(0xA8, 0x34, 0x5B).ToHSV().Should().BeApproximately(new HSVColour(339.828, 0.69047, 0.65882), 0.001);
        }

        [TestMethod]
        [TestCategory(nameof(HSVColour))]
        [TestCategory(nameof(SRGBColour))]
        public void Test_HSV_to_RGB()
        {
            // Extremes
            new HSVColour(  0, 0, 0, 1).ToSRGB().Should().Be(new SRGBColour(0x00, 0x00, 0x00), "black");
            new HSVColour(  0, 0, 1, 1).ToSRGB().Should().Be(new SRGBColour(0xff, 0xff, 0xff), "white");
            new HSVColour(  0, 1, 1, 1).ToSRGB().Should().Be(new SRGBColour(0xff, 0x00, 0x00), "red");
            new HSVColour(120, 1, 1, 1).ToSRGB().Should().Be(new SRGBColour(0x00, 0xff, 0x00), "green");
            new HSVColour(240, 1, 1, 1).ToSRGB().Should().Be(new SRGBColour(0x00, 0x00, 0xff), "blue");

            // Alpha should pass through
            new HSVColour(0, 0, 0, 0.41).ToSRGB().Should().Be(new SRGBColour(0x00, 0x00, 0x00, 0.41 * 255.0), "black with alpha");

            // Arbitrary colours
            // Test values taken from http://colorizer.org/
            new HSVColour( 59.21, 0.651, 0.862).ToSRGB().Should().BeApproximately(new SRGBColour(0xdc, 0xda, 0x4d));
            new HSVColour(232.28, 0.048, 0.272).ToSRGB().Should().BeApproximately(new SRGBColour(0x42, 0x42, 0x45));
            new HSVColour(251.32, 0.492, 0.429).ToSRGB().Should().BeApproximately(new SRGBColour(0x42, 0x38, 0x6d));
            new HSVColour(108.04, 0.705, 0.833).ToSRGB().Should().BeApproximately(new SRGBColour(0x5d, 0xd4, 0x3f));
            new HSVColour(328.51, 0.035, 0.316).ToSRGB().Should().BeApproximately(new SRGBColour(0x51, 0x4e, 0x4f));
        }

        [TestMethod]
        [TestCategory(nameof(HSLColour))]
        [TestCategory(nameof(SRGBColour))]
        public void Test_RGB_to_HSL()
        {
            // Extremes
            new SRGBColour(0x00, 0x00, 0x00).ToHSL().Should().Be(new HSLColour(0, 0, 0, 1), "black");
            new SRGBColour(0xff, 0xff, 0xff).ToHSL().Should().Be(new HSLColour(0, 0, 1, 1), "white");
            new SRGBColour(0xff, 0x00, 0x00).ToHSL().Should().Be(new HSLColour(0, 1, 0.5, 1), "red");
            new SRGBColour(0x00, 0xff, 0x00).ToHSL().Should().Be(new HSLColour(120, 1, 0.5, 1), "green");
            new SRGBColour(0x00, 0x00, 0xff).ToHSL().Should().Be(new HSLColour(240, 1, 0.5, 1), "blue");

            // Alpha should pass through
            new SRGBColour(0x00, 0x00, 0x00, 79).ToHSL().Should().Be(new HSLColour(0, 0, 0, 79.0 / 255.0), "black with alpha");

            // Arbitrary colours
            // Test values taken from http://www.easyrgb.com/en/convert.php
            new SRGBColour(0x0C, 0x66, 0xEE).ToHSL().Should().BeApproximately(new HSLColour(216.106, 0.90400, 0.49020), 0.001);
            new SRGBColour(0x38, 0x08, 0x68).ToHSL().Should().BeApproximately(new HSLColour(270.000, 0.85714, 0.21961), 0.001);
            new SRGBColour(0x38, 0xC9, 0x5F).ToHSL().Should().BeApproximately(new HSLColour(136.138, 0.57313, 0.50392), 0.001);
            new SRGBColour(0xA8, 0x34, 0x5B).ToHSL().Should().BeApproximately(new HSLColour(339.828, 0.52727, 0.43137), 0.001);
        }
    }
}
