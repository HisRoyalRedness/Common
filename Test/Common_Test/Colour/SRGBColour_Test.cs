using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;
using HisRoyalRedness.com.ColourConstants;
using System.Windows.Media;

namespace HisRoyalRedness.com.Tests
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    [TestClass]
    public class SRGBColour_Test
    {
        [TestMethod]
        [TestCategory(nameof(SRGBColour))]
        public void Test_SRGBColour_Construct()
        {
            var rgb = new SRGBColour();
            rgb.R.Should().Be(0);
            rgb.G.Should().Be(0);
            rgb.B.Should().Be(0);
            rgb.A.Should().Be(0); // Blegh!!

            rgb = new SRGBColour(11,22,33);
            rgb.R.Should().Be(11);
            rgb.G.Should().Be(22);
            rgb.B.Should().Be(33);
            rgb.A.Should().Be(255);

            rgb = new SRGBColour(11, 22, 33, 44);
            rgb.R.Should().Be(11);
            rgb.G.Should().Be(22);
            rgb.B.Should().Be(33);
            rgb.A.Should().Be(44);
        }

        [TestMethod]
        [TestCategory(nameof(SRGBColour))]
        [TestCategory(nameof(ColourVector))]
        public void Test_SRGBColour_Implicit_cast()
        {
            ((ColourVector)new SRGBColour(11, 22, 33, 44)).Should().Be(new ColourVector(11,22,33));
            ((Color)new SRGBColour(11, 22, 33, 44)).Should().Be(Color.FromArgb(44, 11, 22, 33));
        }

        [TestMethod]
        [TestCategory(nameof(SRGBColour))]
        public void Test_SRGBColour_Add()
        {
            (new SRGBColour(11, 22, 33, 44) + new SRGBColour(55, 66, 77, 88)).Should().Be(new SRGBColour(66, 88, 110, 132));
            (new SRGBColour(55, 66, 77, 88) + new SRGBColour(254, 254, 254)).Should().Be(new SRGBColour(255, 255, 255, 255));
        }

        [TestMethod]
        [TestCategory(nameof(SRGBColour))]
        public void Test_SRGBColour_Subtract()
        {
            (new SRGBColour(55, 67, 79, 91) - new SRGBColour(11, 22, 33, 44)).Should().Be(new SRGBColour(44, 45, 46, 47));
            (new SRGBColour(11, 22, 33, 44) - new SRGBColour(55, 66, 77, 88)).Should().Be(new SRGBColour(0, 0, 0, 0));
        }
    }
}
