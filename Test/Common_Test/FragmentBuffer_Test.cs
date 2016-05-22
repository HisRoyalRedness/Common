using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace HisRoyalRedness.com.Tests
{
    [TestClass]
    public class FragmentBuffer_Test
    {
        #region TestBasicConstruction
        [TestMethod]
        public void TestBasicConstruction()
        {
            // Test each of the 3 supported constructors, i.e.
            // 1.   FragmentBuffer<int>();
            // 2.   FragmentBuffer<int>(int[], offset, length);
            // 3.   FragmentBuffer<int>(FrameBuffer<int>, offset, length);

            // 1.
            {
                var fb = new FragmentBuffer<int>();
                fb.Count.Should().Be(0, "we created an empty buffer");
                fb.Should().Equal(new int[0]);
            }

            // 2.
            {
                var source = new[] { 1, 2, 3, 4, 5 };
                var offset = 0;
                var length = source.Length;

                var fb = new FragmentBuffer<int>(source, offset, length);
                fb.Count.Should().Be(length);
                fb.Offset.Should().Be(offset);
                fb.Should().Equal(1, 2, 3, 4, 5);

                offset = 1;
                length = source.Length - 2;
                fb = new FragmentBuffer<int>(source, offset, length);
                fb.Count.Should().Be(length);
                fb.Offset.Should().Be(offset);
                fb.Should().Equal(2, 3, 4);
            }

            // 3.
            {
                var source = new FragmentBuffer<int>(new[] { 1, 2, 3, 4, 5 }, 0, 5);
                var offset = 0;
                var length = source.Count;

                var fb = new FragmentBuffer<int>(source, offset, length);
                fb.Count.Should().Be(length);
                fb.Offset.Should().Be(offset);
                fb.Should().Equal(1, 2, 3, 4, 5);

                offset = 1;
                length = source.Count - 2;
                fb = new FragmentBuffer<int>(source, offset, length);
                fb.Count.Should().Be(length);
                fb.Offset.Should().Be(offset);
                fb.Should().Equal(2, 3, 4);
            }

            // Test Exceptions
            {
                Action<int[], int, int> construct2 = (s, o, l) => new FragmentBuffer<int>(s, o, l);
                construct2.ShouldThrow<ArgumentNullException, int[], int, int>(null, 1, 1, "source can't be null");
                construct2.ShouldThrow<ArgumentOutOfRangeException, int[], int, int>(new int[5], -1, 1, "offset can't be negative");
                construct2.ShouldThrow<ArgumentOutOfRangeException, int[], int, int>(new int[5], 1, -1, "length can't be negative");
                construct2.ShouldThrow<InvalidOperationException, int[], int, int>(new int[5], 3, 3, "offset + length must be smaller than source");

                Action<FragmentBuffer<int>, int, int> construct3 = (s, o, l) => new FragmentBuffer<int>(s, o, l);
                var fb = new FragmentBuffer<int>(new int[5], 0, 5);
                construct3.ShouldThrow<ArgumentNullException, FragmentBuffer<int>, int, int>(null, 1, 1, "source can't be null");
                construct3.ShouldThrow<ArgumentOutOfRangeException, FragmentBuffer<int>, int, int>(fb, -1, 1, "offset can't be negative");
                construct3.ShouldThrow<ArgumentOutOfRangeException, FragmentBuffer<int>, int, int>(fb, 1, -1, "length can't be negative");
                construct3.ShouldThrow<InvalidOperationException, FragmentBuffer<int>, int, int>(fb, 3, 3, "offset + length must be smaller than source");
            }
        }
        #endregion TestConstruction
    }
}
