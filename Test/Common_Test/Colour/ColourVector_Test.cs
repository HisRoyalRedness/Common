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
    public class ColourVector_Test
    {
        [TestCategory("ColourVector")]
        [TestMethod]
        public void Test_ColourVector_Construction()
        {
            var v = new ColourVector(1, 2, 3);
            v.X.Should().Be(1);
            v.Y.Should().Be(2);
            v.Z.Should().Be(3);
        }

        [TestCategory("ColourVector")]
        [TestMethod]
        public void Test_ColourVector_Add()
        {
            var v1 = new ColourVector(1, 2, 3);
            var v2 = new ColourVector(4, 5, 6);
            (v1 + v2).Should().Be(new ColourVector(5, 7, 9));
        }

        [TestCategory("ColourVector")]
        [TestMethod]
        public void Test_ColourVector_Subtract()
        {
            var v1 = new ColourVector(1, 2, 6);
            var v2 = new ColourVector(4, 5, 3);
            (v2 - v1).Should().Be(new ColourVector(3, 3, -3));
            (v2 - v2).Should().Be(new ColourVector(0, 0, 0));
        }

        [TestCategory("ColourVector")]
        [TestMethod]
        public void Test_ColourVector_Multiply()
        {
            var v1 = new ColourVector(1, 2, 3);
            (v1 * 2.0).Should().Be(new ColourVector(2, 4, 6));
            (2.0 * v1).Should().Be(new ColourVector(2, 4, 6));
        }

        [TestCategory("ColourVector")]
        [TestMethod]
        public void Test_ColourVector_Divide()
        {
            var v1 = new ColourVector(1, 2, 3);
            (v1 / 2.0).Should().Be(new ColourVector(0.5, 1, 1.5));
        }

        [TestCategory("ColourVector")]
        [TestMethod]
        public void Test_ColourVector_Alias()
        {
            var v1 = new ColourVector(1, 2, 3);
            v1.X.Should().Be(v1.R);
            v1.Y.Should().Be(v1.G);
            v1.Z.Should().Be(v1.B);
        }        
    }
}
