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
        public void Test_DegreeColourComponent_ToUnitComponent()
        {
            new DegreeColourComponent(0).ToUnitColour().Should().Be(ColourSpaceConstants.ZERO);
            new DegreeColourComponent(23).ToUnitColour().Should().BeApproximately((ColourPrimitive)23.0 / ColourSpaceConstants.THREE_SIXTY);
            new DegreeColourComponent(101).ToUnitColour().Should().BeApproximately((ColourPrimitive)101.0 / ColourSpaceConstants.THREE_SIXTY);
            new DegreeColourComponent(359.9).ToUnitColour().Should().BeApproximately((ColourPrimitive)359.9 / ColourSpaceConstants.THREE_SIXTY);
            new DegreeColourComponent(360).ToUnitColour().Should().BeApproximately(ColourSpaceConstants.ZERO);
        }

        [TestMethod]
        public void Test_DegreeColourComponent_Add()
        {
            const ColourPrimitive ONE_HUNDRED = (ColourPrimitive)100;
            const ColourPrimitive TWO_HUNDRED = (ColourPrimitive)200;
            const ColourPrimitive THREE_HUNDRED = ONE_HUNDRED + TWO_HUNDRED;
            const ColourPrimitive FOURTY = THREE_HUNDRED + ONE_HUNDRED - ColourSpaceConstants.THREE_SIXTY;

            (new DegreeColourComponent(ONE_HUNDRED) + new DegreeColourComponent(TWO_HUNDRED)).Should().BeApproximately(THREE_HUNDRED);
            (new DegreeColourComponent(ONE_HUNDRED) + TWO_HUNDRED).Should().BeApproximately(THREE_HUNDRED);
            (ONE_HUNDRED + new DegreeColourComponent(TWO_HUNDRED)).Should().BeApproximately(THREE_HUNDRED);

            (new DegreeColourComponent(ONE_HUNDRED) + new DegreeColourComponent(THREE_HUNDRED)).Should().Be(FOURTY);
            (new DegreeColourComponent(ONE_HUNDRED) + THREE_HUNDRED).Should().Be(FOURTY);
            (ONE_HUNDRED + new DegreeColourComponent(THREE_HUNDRED)).Should().Be(FOURTY);
        }

        [TestMethod]
        public void Test_DegreeColourComponent_Subtract()
        {
            const ColourPrimitive ONE_HUNDRED = (ColourPrimitive)100;
            const ColourPrimitive TWO_HUNDRED = (ColourPrimitive)200;
            const ColourPrimitive TWO_SIXTY = ColourSpaceConstants.THREE_SIXTY - ONE_HUNDRED;

            (new DegreeColourComponent(TWO_HUNDRED) - new DegreeColourComponent(ONE_HUNDRED)).Should().BeApproximately(ONE_HUNDRED);
            (new DegreeColourComponent(TWO_HUNDRED) - ONE_HUNDRED).Should().BeApproximately(ONE_HUNDRED);
            (TWO_HUNDRED - new DegreeColourComponent(ONE_HUNDRED)).Should().BeApproximately(ONE_HUNDRED);

            (new DegreeColourComponent(ONE_HUNDRED) - new DegreeColourComponent(TWO_HUNDRED)).Should().Be(TWO_SIXTY);
            (new DegreeColourComponent(ONE_HUNDRED) - TWO_HUNDRED).Should().Be(TWO_SIXTY);
            (ONE_HUNDRED - new DegreeColourComponent(TWO_HUNDRED)).Should().Be(TWO_SIXTY);
        }
    }
}
