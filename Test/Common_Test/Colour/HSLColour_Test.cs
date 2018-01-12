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
    public class HSLColour_Test
    {
        [TestMethod]
        [TestCategory(nameof(HSLColour))]
        public void Test_HSLColour_Construct()
        {
            var hsl = new HSLColour();
            hsl.H.Should().Be(0);
            hsl.S.Should().Be(0);
            hsl.L.Should().Be(0);
            hsl.A.Should().Be(0); // Blegh!!

            hsl = new HSLColour(11, 0.22, 0.33);
            hsl.H.Should().Be((ColourPrimitive)11);
            hsl.S.Should().Be((ColourPrimitive)0.22);
            hsl.L.Should().Be((ColourPrimitive)0.33);
            hsl.A.Should().Be(1);

            hsl = new HSLColour(11, 0.22, 0.33, 0.44);
            hsl.H.Should().Be((ColourPrimitive)11);
            hsl.S.Should().Be((ColourPrimitive)0.22);
            hsl.L.Should().Be((ColourPrimitive)0.33);
            hsl.A.Should().Be((ColourPrimitive)0.44);
        }

        [TestMethod]
        [TestCategory(nameof(HSLColour))]
        [TestCategory(nameof(ColourVector))]
        public void Test_HSLColour_Implicit_cast()
        {
            ((ColourVector)new HSLColour(11, 0.22, 0.33, 0.44)).Should().BeApproximately(new ColourVector(11, 0.22, 0.33));
        }

        [TestMethod]
        [TestCategory(nameof(HSLColour))]
        public void Test_HSLColour_Add()
        {
            (new HSLColour(11, 0.22, 0.33, 0.44) + new HSLColour(55, 0.66, 0.11, 0.11)).Should().BeApproximately(new HSLColour(66, 0.88, 0.44, 0.55));
            (new HSLColour(120, 0.66, 0.77, 0.88) + new HSLColour(300, 0.99, 0.99)).Should().Be(new HSLColour(60, 1, 1, 1));
        }

        [TestMethod]
        [TestCategory(nameof(HSLColour))]
        public void Test_HSLColour_Subtract()
        {
            (new HSLColour(55, 0.67, 0.79, 0.91) - new HSLColour(11, 0.22, 0.33, 0.44)).Should().BeApproximately(new HSLColour(44, 0.45, 0.46, 0.47), 1E-10);
            (new HSLColour(10, 0.22, 0.33, 0.44) - new HSLColour(20, 0.66, 0.77, 0.88)).Should().Be(new HSLColour(350, 0, 0, 0));
        }
    }
}
