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
    public class UnitColourComponent_Test
    {
        [TestMethod]
        public void Test_UnitColourComponent_Construct()
        {
            new UnitColourComponent().Should().Be(ColourSpaceConstants.ZERO);
            UnitColourComponent.MinValue.Should().Be(ColourSpaceConstants.ZERO);
            UnitColourComponent.MaxValue.Should().Be(ColourSpaceConstants.ONE);
            new UnitColourComponent((ColourPrimitive)0.3).Should().BeApproximately((ColourPrimitive)0.3);
        }

        [TestMethod]
        public void Test_UnitColourComponent_ImplicitCast()
        {
            UnitColourComponent colourComp = (UnitColourComponent)(ColourPrimitive)0.71;
            colourComp.Should().BeApproximately((ColourPrimitive)0.71);

            ColourPrimitive colourComp2 = (ColourPrimitive)(new UnitColourComponent((ColourPrimitive)0.101));
            colourComp2.Should().Be((ColourPrimitive)0.101);
        }

        [TestMethod]
        public void Test_UnitColourComponent_RangeClipping()
        {
            new UnitColourComponent(-1.0).Should().Be(ColourSpaceConstants.ZERO);
            new UnitColourComponent(0).Should().Be(ColourSpaceConstants.ZERO);
            new UnitColourComponent(0.1).Should().BeApproximately((ColourPrimitive)0.1);
            new UnitColourComponent(0.9).Should().BeApproximately((ColourPrimitive)0.9);
            new UnitColourComponent(1).Should().Be(ColourSpaceConstants.ONE);
            new UnitColourComponent(10).Should().Be(ColourSpaceConstants.ONE);
        }

        [TestMethod]
        public void Test_UnitColourComponent_ToByteComponent()
        {
            new UnitColourComponent(0).ToByteColour().Should().Be(0);
            new UnitColourComponent(0.101).ToByteColour().Should().BeApproximately((byte)((ColourPrimitive)0.101 * ColourSpaceConstants.TWO_FIVE_FIVE));
            new UnitColourComponent(0.23).ToByteColour().Should().BeApproximately((byte)((ColourPrimitive)0.23 * ColourSpaceConstants.TWO_FIVE_FIVE));
            new UnitColourComponent(1).ToByteColour().Should().Be(255);
        }

        [TestMethod]
        public void Test_UnitColourComponent_Add()
        {
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
        public void Test_UnitColourComponent_Subtract()
        {
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
