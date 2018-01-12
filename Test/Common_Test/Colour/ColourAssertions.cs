using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisRoyalRedness.com.Tests
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    #region ColourMatrix assertions
    public class ColourMatrixAssertions : ReferenceTypeAssertions<ColourMatrix, ColourMatrixAssertions>
    {
        public ColourMatrixAssertions(ColourMatrix value)
        {
            Subject = value;
        }

        protected override string Context => nameof(ColourMatrix);

        public AndConstraint<ColourMatrixAssertions> Be(ColourMatrix expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.M11, nameof(expected.M11), Subject.M11)
                .TestElement(expected.M12, nameof(expected.M12), Subject.M12)
                .TestElement(expected.M13, nameof(expected.M13), Subject.M13)
                .TestElement(expected.M21, nameof(expected.M21), Subject.M21)
                .TestElement(expected.M22, nameof(expected.M22), Subject.M22)
                .TestElement(expected.M23, nameof(expected.M23), Subject.M23)
                .TestElement(expected.M31, nameof(expected.M31), Subject.M31)
                .TestElement(expected.M32, nameof(expected.M32), Subject.M32)
                .TestElement(expected.M33, nameof(expected.M33), Subject.M33);
            return new AndConstraint<ColourMatrixAssertions>(this);
        }

        public AndConstraint<ColourMatrixAssertions> BeApproximately(ColourMatrix expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.M11, nameof(expected.M11), Subject.M11, precision)
                .TestElementApprox(expected.M12, nameof(expected.M12), Subject.M12, precision)
                .TestElementApprox(expected.M13, nameof(expected.M13), Subject.M13, precision)
                .TestElementApprox(expected.M21, nameof(expected.M21), Subject.M21, precision)
                .TestElementApprox(expected.M22, nameof(expected.M22), Subject.M22, precision)
                .TestElementApprox(expected.M23, nameof(expected.M23), Subject.M23, precision)
                .TestElementApprox(expected.M31, nameof(expected.M31), Subject.M31, precision)
                .TestElementApprox(expected.M32, nameof(expected.M32), Subject.M32, precision)
                .TestElementApprox(expected.M33, nameof(expected.M33), Subject.M33, precision);
            return new AndConstraint<ColourMatrixAssertions>(this);
        }
    }
    #endregion ColourMatrix assertions

    #region ColourVector assertions
    public class ColourVectorAssertions : ReferenceTypeAssertions<ColourVector, ColourVectorAssertions>
    {
        public ColourVectorAssertions(ColourVector value)
        {
            Subject = value;
        }

        protected override string Context => nameof(ColourVector);

        public AndConstraint<ColourVectorAssertions> Be(ColourVector expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.X, nameof(expected.X), Subject.X)
                .TestElement(expected.Y, nameof(expected.Y), Subject.Y)
                .TestElement(expected.Z, nameof(expected.Z), Subject.Z);
            return new AndConstraint<ColourVectorAssertions>(this);
        }

        public AndConstraint<ColourVectorAssertions> BeApproximately(ColourVector expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.X, nameof(expected.X), Subject.X, precision)
                .TestElementApprox(expected.Y, nameof(expected.Y), Subject.Y, precision)
                .TestElementApprox(expected.Z, nameof(expected.Z), Subject.Z, precision);
            return new AndConstraint<ColourVectorAssertions>(this);
        }
    }
    #endregion ColourVector assertions

    #region ByteColourComponent assertions
    public class ByteColourComponentAssertions : ReferenceTypeAssertions<ByteColourComponent, ByteColourComponentAssertions>
    {
        public ByteColourComponentAssertions(ByteColourComponent value)
        {
            Subject = value;
        }

        protected override string Context => nameof(ByteColourComponent);

        public AndConstraint<ByteColourComponentAssertions> Be(byte expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected, nameof(expected), Subject.Value);
            return new AndConstraint<ByteColourComponentAssertions>(this);
        }

        public AndConstraint<ByteColourComponentAssertions> BeApproximately(byte expected, byte precision = 1, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected, nameof(expected), Subject.Value, precision);
            return new AndConstraint<ByteColourComponentAssertions>(this);
        }
    }
    #endregion ByteColourComponent assertions

    #region UnitColourComponent assertions
    public class UnitColourComponentAssertions : ReferenceTypeAssertions<UnitColourComponent, UnitColourComponentAssertions>
    {
        public UnitColourComponentAssertions(UnitColourComponent value)
        {
            Subject = value;
        }

        protected override string Context => nameof(UnitColourComponent);

        public AndConstraint<UnitColourComponentAssertions> Be(ColourPrimitive expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected, nameof(expected), Subject.Value);
            return new AndConstraint<UnitColourComponentAssertions>(this);
        }

        public AndConstraint<UnitColourComponentAssertions> BeApproximately(ColourPrimitive expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected, nameof(expected), Subject.Value, precision);
            return new AndConstraint<UnitColourComponentAssertions>(this);
        }
    }
    #endregion UnitColourComponent assertions

    #region DegreeColourComponent assertions
    public class DegreeColourComponentAssertions : ReferenceTypeAssertions<DegreeColourComponent, DegreeColourComponentAssertions>
    {
        public DegreeColourComponentAssertions(DegreeColourComponent value)
        {
            Subject = value;
        }

        protected override string Context => nameof(DegreeColourComponent);

        public AndConstraint<DegreeColourComponentAssertions> Be(ColourPrimitive expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected, nameof(expected), Subject.Value);
            return new AndConstraint<DegreeColourComponentAssertions>(this);
        }

        public AndConstraint<DegreeColourComponentAssertions> BeApproximately(ColourPrimitive expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected, nameof(expected), Subject.Value, precision);
            return new AndConstraint<DegreeColourComponentAssertions>(this);
        }
    }
    #endregion DegreeColourComponent assertions

    #region SRGBColour assertions
    public class SRGBColourAssertions : ReferenceTypeAssertions<SRGBColour, SRGBColourAssertions>
    {
        public SRGBColourAssertions(SRGBColour value)
        {
            Subject = value;
        }

        protected override string Context => nameof(SRGBColour);

        public AndConstraint<SRGBColourAssertions> Be(SRGBColour expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.R, nameof(expected.R), Subject.R)
                .TestElement(expected.G, nameof(expected.G), Subject.G)
                .TestElement(expected.B, nameof(expected.B), Subject.B)
                .TestElement(expected.A, nameof(expected.A), Subject.A);
            return new AndConstraint<SRGBColourAssertions>(this);
        }

        public AndConstraint<SRGBColourAssertions> BeApproximately(SRGBColour expected, int precision = 1, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.R, nameof(expected.R), Subject.R, precision)
                .TestElementApprox(expected.G, nameof(expected.G), Subject.G, precision)
                .TestElementApprox(expected.B, nameof(expected.B), Subject.B, precision)
                .TestElementApprox(expected.A, nameof(expected.A), Subject.A, precision);
            return new AndConstraint<SRGBColourAssertions>(this);
        }

        public AndConstraint<SRGBColourAssertions> BeIgnoringAlpha(SRGBColour expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.R, nameof(expected.R), Subject.R)
                .TestElement(expected.G, nameof(expected.G), Subject.G)
                .TestElement(expected.B, nameof(expected.B), Subject.B);
            return new AndConstraint<SRGBColourAssertions>(this);
        }

        public AndConstraint<SRGBColourAssertions> BeApproximatelyIgnoringAlpha(SRGBColour expected, int precision = 1, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.R, nameof(expected.R), Subject.R, precision)
                .TestElementApprox(expected.G, nameof(expected.G), Subject.G, precision)
                .TestElementApprox(expected.B, nameof(expected.B), Subject.B, precision);
            return new AndConstraint<SRGBColourAssertions>(this);
        }
    }
    #endregion SRGBColour assertions

    #region HSVColour assertions
    public class HSVColourAssertions : ReferenceTypeAssertions<HSVColour, HSVColourAssertions>
    {
        public HSVColourAssertions(HSVColour value)
        {
            Subject = value;
        }

        protected override string Context => nameof(HSVColour);

        public AndConstraint<HSVColourAssertions> Be(HSVColour expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.H, nameof(expected.H), Subject.H)
                .TestElement(expected.S, nameof(expected.S), Subject.S)
                .TestElement(expected.V, nameof(expected.V), Subject.V)
                .TestElement(expected.A, nameof(expected.A), Subject.A);
            return new AndConstraint<HSVColourAssertions>(this);
        }

        public AndConstraint<HSVColourAssertions> BeApproximately(HSVColour expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.H, nameof(expected.H), Subject.H, precision)
                .TestElementApprox(expected.S, nameof(expected.S), Subject.S, precision)
                .TestElementApprox(expected.V, nameof(expected.V), Subject.V, precision)
                .TestElementApprox(expected.A, nameof(expected.A), Subject.A, precision);
            return new AndConstraint<HSVColourAssertions>(this);
        }

        public AndConstraint<HSVColourAssertions> BeIgnoringAlpha(HSVColour expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.H, nameof(expected.H), Subject.H)
                .TestElement(expected.S, nameof(expected.S), Subject.S)
                .TestElement(expected.V, nameof(expected.V), Subject.V);
            return new AndConstraint<HSVColourAssertions>(this);
        }

        public AndConstraint<HSVColourAssertions> BeApproximatelyIgoringAlpha(HSVColour expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.H, nameof(expected.H), Subject.H, precision)
                .TestElementApprox(expected.S, nameof(expected.S), Subject.S, precision)
                .TestElementApprox(expected.V, nameof(expected.V), Subject.V, precision);
            return new AndConstraint<HSVColourAssertions>(this);
        }
    }
    #endregion HSVColour assertions

    #region HSLColour assertions
    public class HSLColourAssertions : ReferenceTypeAssertions<HSLColour, HSLColourAssertions>
    {
        public HSLColourAssertions(HSLColour value)
        {
            Subject = value;
        }

        protected override string Context => nameof(HSLColour);

        public AndConstraint<HSLColourAssertions> Be(HSLColour expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.H, nameof(expected.H), Subject.H)
                .TestElement(expected.S, nameof(expected.S), Subject.S)
                .TestElement(expected.L, nameof(expected.L), Subject.L)
                .TestElement(expected.A, nameof(expected.A), Subject.A);
            return new AndConstraint<HSLColourAssertions>(this);
        }

        public AndConstraint<HSLColourAssertions> BeApproximately(HSLColour expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.H, nameof(expected.H), Subject.H, precision)
                .TestElementApprox(expected.S, nameof(expected.S), Subject.S, precision)
                .TestElementApprox(expected.L, nameof(expected.L), Subject.L, precision)
                .TestElementApprox(expected.A, nameof(expected.A), Subject.A, precision);
            return new AndConstraint<HSLColourAssertions>(this);
        }

        public AndConstraint<HSLColourAssertions> BeIgnoringAlpha(HSLColour expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElement(expected.H, nameof(expected.H), Subject.H)
                .TestElement(expected.S, nameof(expected.S), Subject.S)
                .TestElement(expected.L, nameof(expected.L), Subject.L);
            return new AndConstraint<HSLColourAssertions>(this);
        }

        public AndConstraint<HSLColourAssertions> BeApproximatelyIgoringAlpha(HSLColour expected, ColourPrimitive precision = (ColourPrimitive)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .TestElementApprox(expected.H, nameof(expected.H), Subject.H, precision)
                .TestElementApprox(expected.S, nameof(expected.S), Subject.S, precision)
                .TestElementApprox(expected.L, nameof(expected.L), Subject.L, precision);
            return new AndConstraint<HSLColourAssertions>(this);
        }
    }
    #endregion HSLColour assertions

    internal static class ColourMatrix_Test_Extensions
    {
        public static ColourMatrixAssertions Should(this ColourMatrix value)
            => new ColourMatrixAssertions(value);
        public static ColourVectorAssertions Should(this ColourVector value)
            => new ColourVectorAssertions(value);

        public static ByteColourComponentAssertions Should(this ByteColourComponent value)
            => new ByteColourComponentAssertions(value);
        public static UnitColourComponentAssertions Should(this UnitColourComponent value)
            => new UnitColourComponentAssertions(value);
        public static DegreeColourComponentAssertions Should(this DegreeColourComponent value)
            => new DegreeColourComponentAssertions(value);

        public static SRGBColourAssertions Should(this SRGBColour value)
            => new SRGBColourAssertions(value);
        public static HSVColourAssertions Should(this HSVColour value)
            => new HSVColourAssertions(value);
        public static HSLColourAssertions Should(this HSLColour value)
            => new HSLColourAssertions(value);

        public static Continuation TestElement(this Continuation continuation, ColourPrimitive expected, string expectedName, ColourPrimitive actual)
            => continuation.Then.TestElement(expected, expectedName, actual);
        public static Continuation TestElement(this AssertionScope scope, ColourPrimitive expected, string expectedName, ColourPrimitive actual)
            => scope.ForCondition(expected == actual).FailWith($"Expected {expectedName} to be {expected}, but it is {actual}.");
        public static Continuation TestElement(this Continuation continuation, int expected, string expectedName, int actual)
            => continuation.Then.TestElement(expected, expectedName, actual);
        public static Continuation TestElement(this AssertionScope scope, int expected, string expectedName, int actual)
            => scope.ForCondition(expected == actual).FailWith($"Expected {expectedName} to be {expected}, but it is {actual}.");

        public static Continuation TestElementApprox(this Continuation continuation, float expected, string expectedName, float actual, float precision)
            => continuation.Then.TestElementApprox(expected, expectedName, actual, precision);
        public static Continuation TestElementApprox(this AssertionScope scope, float expected, string expectedName, float actual, float precision)
            => scope.ForCondition(Math.Abs(expected - actual) <= precision).FailWith($"Expected {expectedName} to approximate {expected} +/-{precision}, but it differed by {Math.Abs(expected - actual)}.");
        public static Continuation TestElementApprox(this Continuation continuation, double expected, string expectedName, double actual, double precision)
            => continuation.Then.TestElementApprox(expected, expectedName, actual, precision);
        public static Continuation TestElementApprox(this AssertionScope scope, double expected, string expectedName, double actual, double precision)
            => scope.ForCondition(Math.Abs(expected - actual) <= precision).FailWith($"Expected {expectedName} to approximate {expected} +/-{precision}, but it differed by {Math.Abs(expected - actual)}.");
        public static Continuation TestElementApprox(this Continuation continuation, int expected, string expectedName, int actual, int precision)
            => continuation.Then.TestElementApprox(expected, expectedName, actual, precision);
        public static Continuation TestElementApprox(this AssertionScope scope, int expected, string expectedName, int actual, int precision)
            => scope.ForCondition(Math.Abs(expected - actual) <= precision).FailWith($"Expected {expectedName} to approximate {expected} +/-{precision}, but it differed by {Math.Abs(expected - actual)}.");
    }
}
