using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;


namespace HisRoyalRedness.com.Tests
{
    using ColourPrimitive = Double;

    [TestClass]
    public class ColourMatrix_Test
    {
        [TestMethod]
        public void Test_Construction()
        {
            var m = new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9);
            m.M11.Should().Be(1);
            m.M12.Should().Be(2);
            m.M13.Should().Be(3);
            m.M21.Should().Be(4);
            m.M22.Should().Be(5);
            m.M23.Should().Be(6);
            m.M31.Should().Be(7);
            m.M32.Should().Be(8);
            m.M33.Should().Be(9);
        }

        [TestMethod]
        public void Test_Determinant()
        {
            new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9).Determinant.Should().Be(0);
            new ColourMatrix(1, 2, 6, 3, 9, 4, 7, 8, 5).Determinant.Should().Be(-195);
            new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1).Determinant.Should().Be(-1);

            var m = new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1);
            m.Determinant.Should().Be(-1);
            m.Determinant.Should().Be(-1, "should be cached when requested a second time");
        }

        [TestMethod]
        public void Test_ScalarMultiplictionAndDivision()
        {
            (new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9) * 2.0).Should().Be(new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18));
            (new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18) * 0.5).Should().Be(new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9));
            (new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18) / 2.0).Should().Be(new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9));

            (2.0 * new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9)).Should().Be(new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18));
            (0.5 * new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18)).Should().Be(new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [TestMethod]
        public void Test_Inverse()
        {
            new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1).Inverse.Should().Be(new ColourMatrix(5, -4, 1, -14, 11, -2, 8, -6, 1));
            new ColourMatrix(5, -4, 1, -14, 11, -2, 8, -6, 1).Inverse.Should().Be(new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1));
        }

        [TestMethod]
        public void Test_VectorMultiplication()
        {
            (new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9) * new CIEXYZColour(1, 1, 1)).Should().Be(new CIEXYZColour(6, 15, 24));
            (new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9) * new CIEXYZColour(3, 2, 1)).Should().Be(new CIEXYZColour(10, 28, 46));
        }
    }

    #region Custom assertions
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

        public AndConstraint<ColourMatrixAssertions> BeApproximately(ColourMatrix expected, double precision = (double)(float.Epsilon), string because = "", params object[] becauseArgs)
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

    public class CIEXYZColourAssertions : ReferenceTypeAssertions<CIEXYZColour, CIEXYZColourAssertions>
    {
        public CIEXYZColourAssertions(CIEXYZColour value)
        {
            Subject = value;
        }

        protected override string Context => nameof(CIEXYZColour);

        public AndConstraint<CIEXYZColourAssertions> Be(CIEXYZColour expected, double precision = (double)(float.Epsilon), string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(expected.X == Subject.X)
                .FailWith($"Expected X to be {expected.X}, but it is {Subject.X}.")
                .Then
                .ForCondition(expected.Y == Subject.Y)
                .FailWith($"Expected Y to be {expected.Y}, but it is {Subject.Y}.")
                .Then
                .ForCondition(expected.Z == Subject.Z)
                .FailWith($"Expected Z to be {expected.Z}, but it is {Subject.Z}.");
            return new AndConstraint<CIEXYZColourAssertions>(this);
        }
    }

    internal static class ColourMatrix_Test_Extensions
    {
        public static ColourMatrixAssertions Should(this ColourMatrix matrix)
            => new ColourMatrixAssertions(matrix);
        public static CIEXYZColourAssertions Should(this CIEXYZColour colour)
            => new CIEXYZColourAssertions(colour);

        public static Continuation TestElement(this Continuation continuation, ColourPrimitive expected, string expectedName, ColourPrimitive actual)
            => continuation.Then.TestElement(expected, expectedName, actual);
        public static Continuation TestElement(this AssertionScope scope, ColourPrimitive expected, string expectedName, ColourPrimitive actual)
            => scope.ForCondition(expected == actual).FailWith($"Expected {expectedName} to be {expected}, but it is {actual}.");

        public static Continuation TestElementApprox(this Continuation continuation, ColourPrimitive expected, string expectedName, ColourPrimitive actual, ColourPrimitive precision)
            => continuation.Then.TestElementApprox(expected, expectedName, actual, precision);
        public static Continuation TestElementApprox(this AssertionScope scope, ColourPrimitive expected, string expectedName, ColourPrimitive actual, ColourPrimitive precision)
            => scope.ForCondition(Math.Abs(expected - actual) < precision).FailWith($"Expected {expectedName} to approximate {expected} +/-{precision}, but it differed by {Math.Abs(expected - actual)}.");
    }
    #endregion Custom assertions
}
