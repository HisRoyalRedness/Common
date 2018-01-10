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
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    [TestClass]
    public class ColourMatrix_Test
    {
        [TestMethod]
        public void Test_ColourMatrix_Construction()
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
        public void Test_ColourMatrix_Determinant()
        {
            new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9).Determinant.Should().Be(0);
            new ColourMatrix(1, 2, 6, 3, 9, 4, 7, 8, 5).Determinant.Should().Be(-195);
            new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1).Determinant.Should().Be(-1);

            var m = new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1);
            m.Determinant.Should().Be(-1);
            m.Determinant.Should().Be(-1, "should be cached when requested a second time");
        }

        [TestMethod]
        public void Test_ColourMatrix_ScalarMultiplictionAndDivision()
        {
            (new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9) * 2.0).Should().Be(new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18));
            (new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18) * 0.5).Should().Be(new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9));
            (new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18) / 2.0).Should().Be(new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9));

            (2.0 * new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9)).Should().Be(new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18));
            (0.5 * new ColourMatrix(2, 4, 6, 8, 10, 12, 14, 16, 18)).Should().Be(new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [TestMethod]
        public void Test_ColourMatrix_Inverse()
        {
            new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1).Inverse.Should().Be(new ColourMatrix(5, -4, 1, -14, 11, -2, 8, -6, 1));
            new ColourMatrix(5, -4, 1, -14, 11, -2, 8, -6, 1).Inverse.Should().Be(new ColourMatrix(1, 2, 3, 2, 3, 4, 4, 2, 1));
        }

        [TestMethod]
        public void Test_ColourMatrix_VectorMultiplication()
        {
            (new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9) * new ColourVector(1, 1, 1)).Should().Be(new ColourVector(6, 15, 24));
            (new ColourMatrix(1, 2, 3, 4, 5, 6, 7, 8, 9) * new ColourVector(3, 2, 1)).Should().Be(new ColourVector(10, 28, 46));
        }
    }
}
