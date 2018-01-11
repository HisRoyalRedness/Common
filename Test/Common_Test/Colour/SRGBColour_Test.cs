using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;
using HisRoyalRedness.com.ColourConstants;

namespace HisRoyalRedness.com.Tests
{
    [TestClass]
    public class SRGBColour_Test
    {
        [TestMethod]
        public void Test_SRGBColour_Construct()
        {
            var rgb = new SRGBColour();
            rgb.R.Should().Be(0);
            rgb.G.Should().Be(0);
            rgb.B.Should().Be(0);
            rgb.A.Should().Be(0); // Blegh!!
        }
    }
}
