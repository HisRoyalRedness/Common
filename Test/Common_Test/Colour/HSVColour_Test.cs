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
    public class HSVColour_Test
    {
        [TestMethod]
        [TestCategory(nameof(HSVColour))]
        public void Test_HSVColour_Construct()
        {
            var hsv = new HSVColour();
            hsv.H.Should().Be(0);
            hsv.S.Should().Be(0);
            hsv.V.Should().Be(0);
            hsv.A.Should().Be(0); // Blegh!!

            hsv = new HSVColour(11, 0.22, 0.33);
            hsv.H.Should().Be((ColourPrimitive)11);
            hsv.S.Should().Be((ColourPrimitive)0.22);
            hsv.V.Should().Be((ColourPrimitive)0.33);
            hsv.A.Should().Be(1);

            hsv = new HSVColour(11, 0.22, 0.33, 0.44);
            hsv.H.Should().Be((ColourPrimitive)11);
            hsv.S.Should().Be((ColourPrimitive)0.22);
            hsv.V.Should().Be((ColourPrimitive)0.33);
            hsv.A.Should().Be((ColourPrimitive)0.44);
        }

        [TestMethod]
        [TestCategory(nameof(HSVColour))]
        [TestCategory(nameof(ColourVector))]
        public void Test_HSVColour_Implicit_cast()
        {
            ((ColourVector)new HSVColour(11, 0.22, 0.33, 0.44)).Should().BeApproximately(new ColourVector(11, 0.22, 0.33));
        }

        [TestMethod]
        [TestCategory(nameof(HSVColour))]
        public void Test_HSVColour_Add()
        {
            (new HSVColour(11, 0.22, 0.33, 0.44) + new HSVColour(55, 0.66, 0.11, 0.11)).Should().BeApproximately(new HSVColour(66, 0.88, 0.44, 0.55));
            (new HSVColour(120, 0.66, 0.77, 0.88) + new HSVColour(300, 0.99, 0.99)).Should().Be(new HSVColour(60, 1, 1, 1));
        }

        [TestMethod]
        [TestCategory(nameof(HSVColour))]
        public void Test_HSVColour_Subtract()
        {
            (new HSVColour(55, 0.67, 0.79, 0.91) - new HSVColour(11, 0.22, 0.33, 0.44)).Should().BeApproximately(new HSVColour(44, 0.45, 0.46, 0.47), 1E-10);
            (new HSVColour(10, 0.22, 0.33, 0.44) - new HSVColour(20, 0.66, 0.77, 0.88)).Should().Be(new HSVColour(350, 0, 0, 0));
        }
    }
}
