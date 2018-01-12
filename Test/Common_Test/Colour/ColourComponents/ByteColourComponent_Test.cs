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

    public partial class ByteColourComponent_Test
    {
        [TestMethod]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Min_and_Max()
        {
            ByteColourComponent.MinValue.Should().Be(0);
            ByteColourComponent.MaxValue.Should().Be(255);
        }

        [TestMethod]
        [TestCategory(nameof(ByteColourComponent))]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_ByteColourComponent_ToUnitComponent()
        {
            new ByteColourComponent(0).ToUnitColour().Should().Be(ColourSpaceConstants.ZERO);
            new ByteColourComponent(23).ToUnitColour().Should().BeApproximately((ColourPrimitive)23.0 / ColourSpaceConstants.TWO_FIVE_FIVE);
            new ByteColourComponent(101).ToUnitColour().Should().BeApproximately((ColourPrimitive)101.0 / ColourSpaceConstants.TWO_FIVE_FIVE);
            new ByteColourComponent(255).ToUnitColour().Should().Be(ColourSpaceConstants.ONE);
        }
    }
}
