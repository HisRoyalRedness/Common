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

    public partial class DegreeColourComponent_Test
    {
        [TestMethod]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Min_and_Max()
        {
            DegreeColourComponent.MinValue.Should().Be(0);
            DegreeColourComponent.MaxValue.Should().Be(360);
        }

        [TestMethod]
        [TestCategory(nameof(DegreeColourComponent))]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_DegreeColourComponent_ToUnitComponent()
        {
            new DegreeColourComponent(0).ToUnitColour().Should().Be(ColourSpaceConstants.ZERO);
            new DegreeColourComponent(23).ToUnitColour().Should().BeApproximately((ColourPrimitive)23.0 / ColourSpaceConstants.THREE_SIXTY);
            new DegreeColourComponent(101).ToUnitColour().Should().BeApproximately((ColourPrimitive)101.0 / ColourSpaceConstants.THREE_SIXTY);
            new DegreeColourComponent(359.9).ToUnitColour().Should().BeApproximately((ColourPrimitive)359.9 / ColourSpaceConstants.THREE_SIXTY);
            new DegreeColourComponent(360).ToUnitColour().Should().BeApproximately(ColourSpaceConstants.ZERO);
        }
    }
}
