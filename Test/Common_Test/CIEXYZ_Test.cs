using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;
using System.Windows.Media;

namespace HisRoyalRedness.com.Tests
{
    using ColourPrimitive = Double;

    [TestClass]
    public class CIEXYZ_Test
    {
        [TestMethod]
        public void Test_CIEXYZColour_construction()
        {
            var xyz = new CIEXYZColour(0.1, 0.2, 0.3);
            xyz.X.Should().Be(0.1);
            xyz.Y.Should().Be(0.2);
            xyz.Z.Should().Be(0.3);
            xyz.Illuminant.Should().Be(Illuminants.D65, "default to D65");

            new Action(() => new CIEXYZColour(0.0, 0.0, 0.0)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(1.0, 0.0, 0.0)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 1.0, 0.0)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 0.0, 1.0)).ShouldNotThrow<ArgumentOutOfRangeException>();


            new Action(() => new CIEXYZColour(0.0 - ColourPrimitive.Epsilon, 0.0, 0.0)).ShouldThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 0.0 - ColourPrimitive.Epsilon, 0.0)).ShouldThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 0.0, 0.0 - ColourPrimitive.Epsilon)).ShouldThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(1.0000001, 0.0, 0.0)).ShouldThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 1.0000001, 0.0)).ShouldThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 0.0, 1.0000001)).ShouldThrow<ArgumentOutOfRangeException>();

            new Action(() => new CIEXYZColour(0.0 - ColourPrimitive.Epsilon, 0.0, 0.0, false)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 0.0 - ColourPrimitive.Epsilon, 0.0, false)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 0.0, 0.0 - ColourPrimitive.Epsilon, false)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(1.0000001, 0.0, 0.0, false)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 1.0000001, 0.0, false)).ShouldNotThrow<ArgumentOutOfRangeException>();
            new Action(() => new CIEXYZColour(0.0, 0.0, 1.0000001, false)).ShouldNotThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void Test_RGB_conversion()
        {
            var rgb = Color.FromRgb(100, 100, 100);
            var xyz = rgb.ToCIEXYZ();
            xyz.Should().BeApproximately(new CIEXYZColour(0.121126, 0.127432, 0.138777), 0.000001);
            xyz.ToRGB().Should().BeApproximately(rgb); // Round trip

        }
    }

    #region Custom assertions
    public class CIEXYZColourAssertions : ReferenceTypeAssertions<CIEXYZColour, CIEXYZColourAssertions>
    {
        public CIEXYZColourAssertions(CIEXYZColour value)
        {
            Subject = value;
        }

        protected override string Context => nameof(CIEXYZColour);

        public AndConstraint<CIEXYZColourAssertions> Be(CIEXYZColour expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.X, nameof(expected.X), Subject.X)
                .TestElement(expected.Y, nameof(expected.Y), Subject.Y)
                .TestElement(expected.Z, nameof(expected.Z), Subject.Z);
            return new AndConstraint<CIEXYZColourAssertions>(this);
        }

        public AndConstraint<CIEXYZColourAssertions> BeApproximately(CIEXYZColour expected, double precision = (double)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.X, nameof(expected.X), Subject.X, precision)
                .TestElementApprox(expected.Y, nameof(expected.Y), Subject.Y, precision)
                .TestElementApprox(expected.Z, nameof(expected.Z), Subject.Z, precision);
            return new AndConstraint<CIEXYZColourAssertions>(this);
        }
    }

    public class RGBColourAssertions : ReferenceTypeAssertions<Color, RGBColourAssertions>
    {
        public RGBColourAssertions(Color value)
        {
            Subject = value;
        }

        protected override string Context => nameof(Color);

        public AndConstraint<RGBColourAssertions> Be(Color expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.R, nameof(expected.R), Subject.R)
                .TestElement(expected.G, nameof(expected.G), Subject.G)
                .TestElement(expected.B, nameof(expected.B), Subject.B);
            return new AndConstraint<RGBColourAssertions>(this);
        }

        public AndConstraint<RGBColourAssertions> BeApproximately(Color expected, int precision = 1, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.R, nameof(expected.R), Subject.R, precision)
                .TestElementApprox(expected.G, nameof(expected.G), Subject.G, precision)
                .TestElementApprox(expected.B, nameof(expected.B), Subject.B, precision);
            return new AndConstraint<RGBColourAssertions>(this);
        }
    }

    internal static class CIEXYZ_Test_Extensions
    {
        public static CIEXYZColourAssertions Should(this CIEXYZColour colour)
            => new CIEXYZColourAssertions(colour);
        public static RGBColourAssertions Should(this Color colour)
            => new RGBColourAssertions(colour);
    }
    #endregion Custom assertions
}
