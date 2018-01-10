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

    [TestClass]
    public class DegreeColourComponent_Test
    {
        [TestMethod]
        public void Test_DegreeColourComponent_Construct()
        {
            new DegreeColourComponent().Should().Be(ColourSpaceConstants.ZERO);
            DegreeColourComponent.MinValue.Should().Be(ColourSpaceConstants.ZERO);
            DegreeColourComponent.MaxValue.Should().Be(ColourSpaceConstants.THREE_SIXTY);
            new DegreeColourComponent((ColourPrimitive)0.3).Should().BeApproximately((ColourPrimitive)0.3);
        }

        [TestMethod]
        public void Test_DegreeColourComponent_ImplicitCast()
        {
            DegreeColourComponent colourComp = (DegreeColourComponent)(ColourPrimitive)71;
            colourComp.Should().BeApproximately((ColourPrimitive)71);

            ColourPrimitive colourComp2 = (ColourPrimitive)(new DegreeColourComponent((ColourPrimitive)101));
            colourComp2.Should().Be((ColourPrimitive)101);
        }

        [TestMethod]
        public void Test_DegreeColourComponent_RangeClipping()
        {
            new DegreeColourComponent(-1.5).Should().Be(358.5);
            new DegreeColourComponent(0).Should().Be(ColourSpaceConstants.ZERO);
            new DegreeColourComponent(1).Should().Be(1);
            new DegreeColourComponent(300).Should().Be(300);
            new DegreeColourComponent(360).Should().Be(ColourSpaceConstants.ZERO);
            new DegreeColourComponent(365).Should().Be(5);
        }

        [TestMethod]
        public void Test_DegreeColourComponent_ToByteComponent()
        {
            fix me
            new UnitColourComponent(0).ToByteColour().Should().Be(0);
            new UnitColourComponent(0.101).ToByteColour().Should().BeApproximately((byte)((ColourPrimitive)0.101 * ColourSpaceConstants.TWO_FIVE_FIVE));
            new UnitColourComponent(0.23).ToByteColour().Should().BeApproximately((byte)((ColourPrimitive)0.23 * ColourSpaceConstants.TWO_FIVE_FIVE));
            new UnitColourComponent(1).ToByteColour().Should().Be(255);
        }

        [TestMethod]
        public void Test_DegreeColourComponent_Add()
        {
            fix me
            const ColourPrimitive POINT_ONE = (ColourPrimitive)0.1;
            const ColourPrimitive POINT_TWO = (ColourPrimitive)0.1;
            const ColourPrimitive POINT_THREE = POINT_ONE + POINT_TWO;
            const ColourPrimitive POINT_NINE = (ColourPrimitive)0.9;

            (new UnitColourComponent(POINT_ONE) + new UnitColourComponent(POINT_TWO)).Should().BeApproximately(POINT_THREE);
            (new UnitColourComponent(POINT_ONE) + POINT_TWO).Should().BeApproximately(POINT_THREE);
            (POINT_ONE + new UnitColourComponent(POINT_TWO)).Should().BeApproximately(POINT_THREE);

            (new UnitColourComponent(POINT_NINE) + new UnitColourComponent(POINT_TWO)).Should().Be(ColourSpaceConstants.ONE);
            (new UnitColourComponent(POINT_NINE) + POINT_TWO).Should().Be(ColourSpaceConstants.ONE);
            (POINT_NINE + new UnitColourComponent(POINT_TWO)).Should().Be(ColourSpaceConstants.ONE);
        }

        [TestMethod]
        public void Test_DegreeColourComponent_Subtract()
        {
            fix me
            const ColourPrimitive POINT_FIVE = (ColourPrimitive)0.5;
            const ColourPrimitive POINT_FOUR = (ColourPrimitive)0.4;
            const ColourPrimitive POINT_ONE = POINT_FIVE - POINT_FOUR;

            (new UnitColourComponent(POINT_FIVE) - new UnitColourComponent(POINT_FOUR)).Should().BeApproximately(POINT_ONE);
            (new UnitColourComponent(POINT_FIVE) - POINT_FOUR).Should().BeApproximately(POINT_ONE);
            (POINT_FIVE - new UnitColourComponent(POINT_FOUR)).Should().BeApproximately(POINT_ONE);

            (new UnitColourComponent(POINT_FOUR) - new UnitColourComponent(POINT_FIVE)).Should().Be(ColourSpaceConstants.ZERO);
            (new UnitColourComponent(POINT_FOUR) - POINT_FIVE).Should().Be(ColourSpaceConstants.ZERO);
            (POINT_FOUR - new UnitColourComponent(POINT_FIVE)).Should().Be(ColourSpaceConstants.ZERO);
        }
    }
}
