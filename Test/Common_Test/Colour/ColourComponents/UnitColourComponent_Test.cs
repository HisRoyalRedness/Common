using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;
using HisRoyalRedness.com.ColourConstants;

namespace HisRoyalRedness.com.Tests
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    public partial class UnitColourComponent_Test
    {
        [TestMethod]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Min_and_Max()
        {
            UnitColourComponent.MinValue.Should().Be(0);
            UnitColourComponent.MaxValue.Should().Be(1);
        }

        [TestMethod]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_ToByteComponent()
        {
            new UnitColourComponent(0).ToByteColour().Should().Be(0);
            new UnitColourComponent(0.101).ToByteColour().Should().BeApproximately((byte)((ColourPrimitive)0.101 * ColourSpaceConstants.TWO_FIVE_FIVE));
            new UnitColourComponent(0.23).ToByteColour().Should().BeApproximately((byte)((ColourPrimitive)0.23 * ColourSpaceConstants.TWO_FIVE_FIVE));
            new UnitColourComponent(1).ToByteColour().Should().Be(255);
        }
    }
}
