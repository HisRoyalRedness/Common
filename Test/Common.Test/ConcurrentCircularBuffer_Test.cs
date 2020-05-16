using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HisRoyalRedness.com
{
    using IntBuffer = ConcurrentCircularBuffer<int>;

    [TestClass]
    public class ConcurrentCircularBuffer_Test
    {
        #region Construction
        [TestMethod]
        public void ConstructABuffer()
        {
            var capacity = IntBuffer.DEFAULT_CAPACITY;
            var overwrite = IntBuffer.DEFAULT_OVERWRITE;
            var cb = new IntBuffer();

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the Overwrite after creating a new {nameof(IntBuffer)} should be {overwrite}");
            cb.Count.Should().Be(0, "the buffer should be empty as we've not added anything.");
        }

        [TestMethod]
        public void ConstructABufferWithCapacity()
        {
            var capacity = 123;
            var overwrite = IntBuffer.DEFAULT_OVERWRITE;
            var cb = new IntBuffer(123);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the Overwrite after creating a new {nameof(IntBuffer)} should be {overwrite}");
            cb.Count.Should().Be(0, "the buffer should be empty as we've not added anything.");
        }

        [TestMethod]
        public void ConstructABufferWithOverwrite()
        {
            var capacity = IntBuffer.DEFAULT_CAPACITY;
            var overwrite = !IntBuffer.DEFAULT_OVERWRITE;
            var cb = new IntBuffer(overwrite);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the Overwrite after creating a new {nameof(IntBuffer)} should be {overwrite}");
            cb.Count.Should().Be(0, "the buffer should be empty as we've not added anything.");
        }

        [TestMethod]
        public void ConstructABufferWithCapacityAndOverwrite()
        {
            var capacity = 456;
            var overwrite = !IntBuffer.DEFAULT_OVERWRITE;
            var cb = new IntBuffer(capacity, overwrite);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the Overwrite after creating a new {nameof(IntBuffer)} should be {overwrite}");
            cb.Count.Should().Be(0, "the buffer should be empty as we've not added anything.");
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsUnderCapacity()
        {
            var capacity = 5;
            var overwrite = false;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity, overwrite);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the Overwrite after creating a new {nameof(IntBuffer)} should be {overwrite}");
            cb.Count.Should().Be(3);
            cb.ToArray().Should().Equal(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsUnderCapacity_WriteIndex()
        {
            var capacity = 5;
            var overwrite = false;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity, overwrite);

            cb.Add(99);
            cb.Count.Should().Be(4);
            cb.ToArray().Should().Equal(new[] { 1, 2, 3, 99 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsUnderCapacity_ReadIndex()
        {
            var capacity = 5;
            var overwrite = false;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity, overwrite);

            cb.Remove().Should().Be(1);
            cb.Count.Should().Be(2);
            cb.ToArray().Should().Equal(new[] { 2, 3 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsAtCapacity()
        {
            var capacity = 5;
            var overwrite = false;
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, capacity, overwrite);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the Overwrite after creating a new {nameof(IntBuffer)} should be {overwrite}");
            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsAtCapacity_WriteIndex()
        {
            var capacity = 5;
            var overwrite = true;
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, capacity, overwrite);

            cb.Add(99);
            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 2, 3, 4, 5, 99 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsAtCapacity_ReadIndex()
        {
            var capacity = 5;
            var overwrite = false;
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, capacity, overwrite);

            cb.Remove().Should().Be(1);
            cb.Count.Should().Be(4);
            cb.ToArray().Should().Equal(new[] { 2, 3, 4, 5 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsOverCapacity()
        {
            var capacity = 5;
            var overwrite = true;
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5, 6 }, capacity, overwrite);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the Overwrite after creating a new {nameof(IntBuffer)} should be {overwrite}");
            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 2, 3, 4, 5, 6 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsOverCapacity_WriteIndex()
        {
            var capacity = 5;
            var overwrite = true;
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5, 6 }, capacity, overwrite);

            cb.Add(99);
            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 3, 4, 5, 6, 99 });
        }

        [TestMethod]
        public void ConstructABufferWithInitialItemsOverCapacity_ReadIndex()
        {
            var capacity = 5;
            var overwrite = true;
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5, 6 }, capacity, overwrite);

            cb.Remove().Should().Be(2);
            cb.Count.Should().Be(4);
            cb.ToArray().Should().Equal(new[] { 3, 4, 5, 6 });
        }

        [TestMethod]
        public void ConstructABufferWithZeroCapacity()
        {
            new Action(() => new IntBuffer(0)).Should().Throw<ArgumentOutOfRangeException>("a zero Capacity is invalid");
        }

        [TestMethod]
        public void ConstructABufferWithNegativeCapacity()
        {
            new Action(() => new IntBuffer(-1)).Should().Throw<ArgumentOutOfRangeException>("a negative Capacity is invalid");
        }

        [TestMethod]
        public void ConstructABufferWithOverCapacity()
        {
            new Action(() => new IntBuffer(new int[] { 1, 2, 3 }, 2)).Should().Throw<ArgumentOutOfRangeException>("more intials items where given than the capacity allows");
        }
        #endregion Construction

        #region Add
        [TestMethod]
        public void AddItemsAtStart()
        {
            var capacity = 5;
            var cb = new IntBuffer(capacity);
            cb.Add(1);
            cb.Add(2);
            cb.Add(3);
            cb.Add(4);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Count.Should().Be(4);
            cb.ToArray().Should().Equal(new[] { 1, 2, 3, 4 });
        }

        [TestMethod]
        public void AddItemsWithInit()
        {
            var capacity = 5;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity);
            cb.Add(4);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Count.Should().Be(4);
            cb.ToArray().Should().Equal(new[] { 1, 2, 3, 4 });
        }

        [TestMethod]
        public void AddItemsOverCapacityWithOverwrite()
        {
            var capacity = 5;
            var cb = new IntBuffer(capacity, true);
            cb.Add(1);
            cb.Add(2);
            cb.Add(3);
            cb.Add(4);
            cb.Add(5);
            cb.Add(6);

            cb.Capacity.Should().Be(capacity, $"the Capacity after creating a new {nameof(IntBuffer)} should be {capacity}");
            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 2, 3, 4, 5, 6 });
        }

        [TestMethod]
        public void AddItemsOverCapacityWithoutOverwrite()
        {
            var capacity = 5;
            var cb = new IntBuffer(capacity);
            cb.Add(1);
            cb.Add(2);
            cb.Add(3);
            cb.Add(4);
            cb.Add(5);

            new Action(() => cb.Add(6)).Should().Throw<InvalidOperationException>();
        }
        #endregion Add

        #region Remove
        [TestMethod]
        public void RemoveInitItems()
        {
            var capacity = 5;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity);

            cb.Remove().Should().Be(1);
            cb.Count.Should().Be(2);
            cb.ToArray().Should().Equal(new[] { 2, 3 });
        }

        [TestMethod]
        public void RemoveAddedItems()
        {
            var capacity = 5;
            var cb = new IntBuffer(capacity);
            cb.Add(1);
            cb.Add(2);
            cb.Add(3);

            cb.Remove().Should().Be(1);
            cb.Count.Should().Be(2);
            cb.ToArray().Should().Equal(new[] { 2, 3 });
        }

        [TestMethod]
        public void RemoveFromEmptyBuffer()
        {
            var capacity = 5;
            var cb = new IntBuffer(capacity);

            new Action(() => cb.Remove()).Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void RemoveMultipleInitItems()
        {
            var capacity = 5;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity);

            cb.Remove(2).Should().Be(2);
            cb.Count.Should().Be(1);
            cb.ToArray().Should().Equal(new[] { 3 });
        }

        [TestMethod]
        public void RemoveMultipleAddedItems()
        {
            var capacity = 5;
            var cb = new IntBuffer(capacity);
            cb.Add(1);
            cb.Add(2);
            cb.Add(3);

            cb.Remove(2).Should().Be(2);
            cb.Count.Should().Be(1);
            cb.ToArray().Should().Equal(new[] { 3 });
        }

        [TestMethod]
        public void RemoveMoreItemsThanAdded()
        {
            var capacity = 5;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity);

            cb.Remove(5).Should().Be(3);
            cb.Count.Should().Be(0);
            cb.ToArray().Length.Should().Be(0);
        }
        #endregion Remove

        #region Clear
        [TestMethod]
        public void ClearBuffer()
        {
            var capacity = 5;
            var cb = new IntBuffer(new[] { 1, 2, 3 }, capacity);
            cb.Add(4);
            cb.Count.Should().Be(4);
            cb.Clear();
            cb.Count.Should().Be(0);
            cb.ToArray().Length.Should().Be(0);
        }
        #endregion Clear

        // Count is implicitly tested through the other tests

        #region Copy
        [TestMethod]
        public void CopyToASmallerArray()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);

            var target = new int[3];
            cb.CopyTo(target, 0, 3);
            target.Should().Equal(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void CopyToASmallerMemory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);

            var target = new int[3];
            cb.CopyTo(new Memory<int>(target, 0, 3)).Should().Be(3);
            target.Should().Equal(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void CopyToASmallerArrayWithOffset()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);

            var target = new int[3];
            cb.CopyTo(target, 1, 2);
            target.Should().Equal(new[] { 0, 1, 2 });
        }

        [TestMethod]
        public void CopyToALargerArray()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);

            var target = new int[10];
            cb.CopyTo(target, 0);
            target.Should().Equal(new[] { 1, 2, 3, 4, 5, 0 ,0, 0, 0, 0 });
        }

        [TestMethod]
        public void CopyToALargerMemory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);

            var target = new int[10];
            cb.CopyTo(new Memory<int>(target)).Should().Be(5);
            target.Should().Equal(new[] { 1, 2, 3, 4, 5, 0, 0, 0, 0, 0 });
        }

        [TestMethod]
        public void CopyToALargerArrayWithOffset()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);

            var target = new int[10];
            cb.CopyTo(target, 3);
            target.Should().Equal(new[] { 0, 0, 0, 1, 2, 3, 4, 5, 0, 0 });
        }

        [TestMethod]
        public void CopyAnEmptyBufferToAnArray()
        {
            var cb = new IntBuffer();

            var target = new int[5];
            cb.CopyTo(target, 0);
            target.Should().Equal(new[] { 0, 0, 0, 0, 0 });
        }

        [TestMethod]
        public void CopyAnEmptyBufferToAMemory()
        {
            var cb = new IntBuffer();

            var target = new int[5];
            cb.CopyTo(new Memory<int>(target)).Should().Be(0);
            target.Should().Equal(new[] { 0, 0, 0, 0, 0 });
        }

        [TestMethod]
        public void CopyToANullArray()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);

            new Action(() => cb.CopyTo(null, 0)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void CopyWithANegativeOffset_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);
            var target = new int[5];
            new Action(() => cb.CopyTo(target, -1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void CopyWithANegativeLength_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);
            var target = new int[5];
            new Action(() => cb.CopyTo(target, 0, -1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void CopyWithAnInvalidLengthAndOffset_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4, 5 }, 5);
            var target = new int[5];
            new Action(() => cb.CopyTo(target, 4, 4)).Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void CopyWrappedSmallerFirstPortionOnly_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3}, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove(); // Write index should be positioned at 3

            cb.Count.Should().Be(0);
            cb.Add(10);
            cb.Add(11);
            cb.Add(12);
            cb.Add(13);

            var target = new int[] { -1, -1, -1, -1, -1 };
            cb.CopyTo(target, 0, 1);
            target.Should().Equal(new[] { 10, -1, -1, -1, -1 });
        }

        [TestMethod]
        public void CopyWrappedFullFirstPortionOnly_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove(); // Write index should be positioned at 3

            cb.Count.Should().Be(0);
            cb.Add(10);
            cb.Add(11);
            cb.Add(12);
            cb.Add(13);

            var target = new int[] { -1, -1, -1, -1, -1 };
            cb.CopyTo(target, 0, 2);
            target.Should().Equal(new[] { 10, 11, -1, -1, -1 });
        }

        [TestMethod]
        public void CopyFullWrappedBuffer_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove(); // Write index should be positioned at 3

            cb.Count.Should().Be(0);
            cb.Add(10);
            cb.Add(11);
            cb.Add(12);
            cb.Add(13);

            var target = new int[] { -1, -1, -1, -1, -1 };
            cb.CopyTo(target, 0);
            target.Should().Equal(new[] { 10, 11, 12, 13, -1 });
        }

        [TestMethod]
        public void CopyWrappedSmallerFirstPortionOnly_Memory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove(); // Write index should be positioned at 3

            cb.Count.Should().Be(0);
            cb.Add(10);
            cb.Add(11);
            cb.Add(12);
            cb.Add(13);

            var target = new int[] { -1, -1, -1, -1, -1 };
            cb.CopyTo(new Memory<int>(target).Slice(0, 1)).Should().Be(1);
            target.Should().Equal(new[] { 10, -1, -1, -1, -1 });
        }

        [TestMethod]
        public void CopyWrappedFullFirstPortionOnly_Memory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove(); // Write index should be positioned at 3

            cb.Count.Should().Be(0);
            cb.Add(10);
            cb.Add(11);
            cb.Add(12);
            cb.Add(13);

            var target = new int[] { -1, -1, -1, -1, -1 };
            cb.CopyTo(new Memory<int>(target).Slice(0, 2)).Should().Be(2);
            target.Should().Equal(new[] { 10, 11, -1, -1, -1 });
        }

        [TestMethod]
        public void CopyFullWrappedBuffer_SmallerMemory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove(); // Write index should be positioned at 3

            cb.Count.Should().Be(0);
            cb.Add(10);
            cb.Add(11);
            cb.Add(12);
            cb.Add(13);

            var target = new int[] { -1, -1, -1, -1, -1 };
            cb.CopyTo(new Memory<int>(target).Slice(0, 3)).Should().Be(3);
            target.Should().Equal(new[] { 10, 11, 12, -1, -1 });
        }

        [TestMethod]
        public void CopyFullWrappedBuffer_LargerMemory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove(); // Write index should be positioned at 3

            cb.Count.Should().Be(0);
            cb.Add(10);
            cb.Add(11);
            cb.Add(12);
            cb.Add(13);

            var target = new int[] { -1, -1, -1, -1, -1, -1 };
            cb.CopyTo(new Memory<int>(target)).Should().Be(4);
            target.Should().Equal(new[] { 10, 11, 12, 13, -1, -1 });
        }
        #endregion Copy

        #region Indexing
        [TestMethod]
        public void BasicIndexing()
        {
            var cb = new IntBuffer(new int[] { 1, 2, 3 }, 5);

            cb[0].Should().Be(1);
            cb[1].Should().Be(2);
            cb[2].Should().Be(3);
        }

        [TestMethod]
        public void IndexAfterARemovePointsToTheNextItem()
        {
            var cb = new IntBuffer(new int[] { 1, 2, 3 }, 5);
            cb[0].Should().Be(1);
            cb.Remove();
            cb[0].Should().Be(2);
        }

        [TestMethod]
        public void NegativeIndexIsRejected()
        {
            var cb = new IntBuffer(new int[] { 1, 2, 3 }, 5);
            int i = 0;
            new Action(() => i = cb[-1]).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void OutOfRangeIndexIsRejected()
        {
            var cb = new IntBuffer(new int[] { 1, 2, 3 }, 5);
            int i = 0;
            new Action(() => i = cb[10]).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void IndexAfterWrapPointsToFirst()
        {
            var cb = new IntBuffer(new int[] { 1, 2, 3, 4, 5, 6 }, 4, true);
            cb[0].Should().Be(3);
        }
        #endregion Indexing

        #region Read
        [TestMethod]
        public void ReadIntoASmallerArray()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4 }, 5);

            var target = new int[] { -1, -1 };
            cb.Read(target, 0, target.Length).Should().Be(2);
            cb.Count.Should().Be(2);
            target.Should().Equal(new[] { 1, 2 });
        }

        [TestMethod]
        public void ReadIntoAnEqualSizeArray()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4 }, 5);

            var target = new int[] { -1, -1, -1, -1 };
            cb.Read(target, 0, target.Length).Should().Be(4);
            cb.Count.Should().Be(0);
            target.Should().Equal(new[] { 1, 2, 3, 4 });
        }

        [TestMethod]
        public void ReadIntoALargerSizeArray()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4 }, 5);

            var target = new int[] { -1, -1, -1, -1, -1, -1 };
            cb.Read(target, 0, target.Length).Should().Be(4);
            cb.Count.Should().Be(0);
            target.Should().Equal(new[] { 1, 2, 3, 4, -1, -1 });
        }

        [TestMethod]
        public void ReadIntoASmallerMemory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4 }, 5);

            var target = new int[] { -1, -1 };
            cb.Read(new Memory<int>(target)).Should().Be(2);
            cb.Count.Should().Be(2);
            target.Should().Equal(new[] { 1, 2 });
        }

        [TestMethod]
        public void ReadIntoAnEqualSizeMemory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4 }, 5);

            var target = new int[] { -1, -1, -1, -1 };
            cb.Read(new Memory<int>(target)).Should().Be(4);
            cb.Count.Should().Be(0);
            target.Should().Equal(new[] { 1, 2, 3, 4 });
        }

        [TestMethod]
        public void ReadIntoALargerSizeMemory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3, 4 }, 5);

            var target = new int[] { -1, -1, -1, -1, -1, -1 };
            cb.Read(new Memory<int>(target)).Should().Be(4);
            cb.Count.Should().Be(0);
            target.Should().Equal(new[] { 1, 2, 3, 4, -1, -1 });
        }
        #endregion Read

        #region Write
        [TestMethod]
        public void Write_Array()
        {
            var cb = new IntBuffer(5);

            var arr = new[] { 1, 2, 3 };
            cb.Write(arr, 0, arr.Length);

            cb.Count.Should().Be(3);
            cb.ToArray().Should().Equal(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void WriteAtCapacity_Array()
        {
            var cb = new IntBuffer(5);

            var arr = new[] { 1, 2, 3, 4, 5 };
            cb.Write(arr, 0, arr.Length);

            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        public void WriteOverCapacityWithOverwrite_Array()
        {
            var cb = new IntBuffer(5, true);

            var arr = new[] { 1, 2, 3, 4, 5, 6, 7 };
            cb.Write(arr, 0, arr.Length);

            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 3, 4, 5, 6, 7 });
        }

        [TestMethod]
        public void WriteOverCapacityWithoutOverwrite_Array()
        {
            var cb = new IntBuffer(5);

            var arr = new[] { 1, 2, 3, 4, 5, 6, 7 };
            new Action(() => cb.Write(arr, 0, arr.Length)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WriteAnEmptyArray()
        {
            var cb = new IntBuffer(new int[] { -1, -1, }, 5);

            var arr = new int[0];
            cb.Write(arr, 0, arr.Length);

            cb.Count.Should().Be(2);
            cb.ToArray().Should().Equal(new[] { -1, -1 });
        }

        [TestMethod]
        public void WriteWithNull_Array()
        {
            var cb = new IntBuffer(5);
            new Action(() => cb.Write(null, 0, 1)).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WriteWithANegativeOffset_Array()
        {
            var cb = new IntBuffer(5);
            new Action(() => cb.Write(new[] { 1 }, -1, 1)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WriteAnInvalidLength_Array()
        {
            var cb = new IntBuffer(5);
            new Action(() => cb.Write(new[] { 1 }, 0, 2)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WriteWithANegativeLength_Array()
        {
            var cb = new IntBuffer(5);
            new Action(() => cb.Write(new[] { 1 }, 0, -2)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WriteWithAnInvalidLengthAndOffset_Array()
        {
            var cb = new IntBuffer(5);
            new Action(() => cb.Write(new[] { 1, 2, 3 }, 2, 2)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WriteWithWrap_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove();

            var arr = new [] { 4, 5, 6, 7 };
            cb.Write(arr, 0, arr.Length);
            cb.Count.Should().Be(4);
            cb.ToArray().Should().Equal(new[] { 4, 5, 6, 7 });
        }

        [TestMethod]
        public void WriteWithLengthOverCapacity_Array()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5, true);
            cb.Remove();
            cb.Remove();
            cb.Remove();

            var arr = new[] { 4, 5, 6, 7, 8, 9, 10, 11 };
            cb.Write(arr, 1, 6);
            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 6, 7, 8, 9, 10 });
        }

        [TestMethod]
        public void WriteWithLengthOverCapacity_Memory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5, true);
            cb.Remove();
            cb.Remove();
            cb.Remove();

            var arr = new[] { 5, 6, 7, 8, 9, 10 };
            cb.Write(new Memory<int>(arr));
            cb.Count.Should().Be(5);
            cb.ToArray().Should().Equal(new[] { 6, 7, 8, 9, 10 });
        }

        [TestMethod]
        public void WriteOverCapacity_Memory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);

            new Action(() => cb.Write(new Memory<int>(new[] { 1, 2, 3, 4 }))).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void WriteOverCapacityWithOverwrite_Memory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5, true);

            cb.Write(new Memory<int>(new[] { 1, 2, 3, 4 })); 
        }

        [TestMethod]
        public void WriteWithWrap_Memory()
        {
            var cb = new IntBuffer(new[] { 1, 2, 3 }, 5);
            cb.Remove();
            cb.Remove();
            cb.Remove();

            var arr = new[] { 4, 5, 6, 7 };
            cb.Write(new Memory<int>(arr));
            cb.Count.Should().Be(4);
            cb.ToArray().Should().Equal(new[] { 4, 5, 6, 7 });
        }
        #endregion Write


        //if (length == 0)
        //    return;
        //if (data == null)
        //    throw new ArgumentNullException(nameof(data));
        //if (offset< 0)
        //    throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be larger or equal to zero.");
        //if (data.Length<length)
        //    throw new ArgumentException($"{nameof(data)} is smaller than {nameof(length)}.");
        //if (data.Length - offset<length)
        //    throw new ArgumentException($"{nameof(data)} is smaller than {nameof(length)} with the provided {nameof(offset)}.");


        // ToArray is implicitly tested through the other tests

    }
}
