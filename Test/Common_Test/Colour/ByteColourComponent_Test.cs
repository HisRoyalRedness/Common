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
    public class ByteColourComponent_Test
    {
        [TestMethod]
        public void Test_ByteColourComponent_Construct()
        {
            new ByteColourComponent().Should().Be(0);
            ByteColourComponent.MinValue.Should().Be(0);
            ByteColourComponent.MaxValue.Should().Be(255);
            new ByteColourComponent(23).Should().Be(23);
        }

        [TestMethod]
        public void Test_ByteColourComponent_ImplicitCast()
        {
            ByteColourComponent colourComp = (ByteColourComponent)(byte)71;
            colourComp.Should().Be(71);

            byte colourComp2 = (byte)(new ByteColourComponent(101));
            colourComp2.Should().Be(101);
        }

        [TestMethod]
        public void Test_ByteColourComponent_RangeClipping()
        {
            new ByteColourComponent(-1).Should().Be(0);
            new ByteColourComponent(0).Should().Be(0);
            new ByteColourComponent(1).Should().Be(1);
            new ByteColourComponent(254).Should().Be(254);
            new ByteColourComponent(255).Should().Be(255);
            new ByteColourComponent(265).Should().Be(255);
        }

        [TestMethod]
        public void Test_ByteColourComponent_ToUnitComponent()
        {
            new ByteColourComponent(0).ToUnitColour().Should().Be(ColourSpaceConstants.ZERO);
            new ByteColourComponent(23).ToUnitColour().Should().BeApproximately((ColourPrimitive)23.0 / ColourSpaceConstants.TWO_FIVE_FIVE);
            new ByteColourComponent(101).ToUnitColour().Should().BeApproximately((ColourPrimitive)101.0 / ColourSpaceConstants.TWO_FIVE_FIVE);
            new ByteColourComponent(255).ToUnitColour().Should().Be(ColourSpaceConstants.ONE);
        }

        [TestMethod]
        public void Test_ByteColourComponent_Add()
        {
            (new ByteColourComponent(1) + new ByteColourComponent(2)).Should().Be(3);
            (new ByteColourComponent(1) + 2).Should().Be(3);
            (1 + new ByteColourComponent(2)).Should().Be(3);

            (new ByteColourComponent(200) + new ByteColourComponent(60)).Should().Be(255);
            (new ByteColourComponent(200) + 60).Should().Be(255);
            (200 + new ByteColourComponent(60)).Should().Be(255);
        }

        [TestMethod]
        public void Test_ByteColourComponent_Subtract()
        {
            (new ByteColourComponent(200) - new ByteColourComponent(100)).Should().Be(100);
            (new ByteColourComponent(200) - 100).Should().Be(100);
            (200 - new ByteColourComponent(100)).Should().Be(100);

            (new ByteColourComponent(100) - new ByteColourComponent(200)).Should().Be(0);
            (new ByteColourComponent(100) - 200).Should().Be(0);
            (100 - new ByteColourComponent(200)).Should().Be(0);
        }
    }
}
