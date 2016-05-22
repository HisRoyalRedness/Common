using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;

namespace HisRoyalRedness.com.Tests
{
    [TestClass]
    public class CircularBuffer 
    {
        #region Construction
        [TestMethod]
        public void Create_an_empty_collection()
        {
            // Arrange
            var defaultCapacity = CircularBuffer<int>.DEFAULT_CAPACITY;
            var defaultOverwrite = CircularBuffer<int>.DEFAULT_OVERWRITE;

            // Act
            var cb = new CircularBuffer<int>();

            // Assert
            cb.Capacity.Should().Be(defaultCapacity, $"the default Capacity after creating an empty collection should be {defaultCapacity}");
            cb.Overwrite.Should().Be(defaultOverwrite, $"the default Overwrite after creating an empty collection should be {defaultOverwrite}");
            cb.Count.Should().Be(0, "the collection should be empty as we've not added anything.");
        }

        [TestMethod]
        public void Create_a_collection_with_a_specific_capacity()
        {
            // Arrange
            var capacity = 100;
            var overwrite = CircularBuffer<int>.DEFAULT_OVERWRITE;
            

            // Act
            var cb = new CircularBuffer<int>(capacity);

            // Assert
            cb.Capacity.Should().Be(capacity, $"the specified Capacity was {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"default Overwrite after construction should be {overwrite}");
            cb.Count.Should().Be(0, "the collection should be empty as we've not added anything.");
        }

        [TestMethod]
        public void Create_a_collection_with_a_secific_capacity_and_overwrite()
        {
            // Arrange
            var capacity = 200;
            var overwrite = true;

            // Act
            var cb = new CircularBuffer<int>(capacity, overwrite);

            // Assert
            cb.Capacity.Should().Be(capacity, $"the specified Capacity was {capacity}");
            cb.Overwrite.Should().Be(overwrite, $"the specified Overwrite was {overwrite}");
            cb.Count.Should().Be(0, "the collection should be empty as we've not added anything.");
        }

        [TestMethod]
        public void Create_a_collection_with_out_of_range_capacity()
        {
            // Arrange
            Action<int> construct = capacity => new CircularBuffer<int>(capacity);

            // Act
            // Assert
            construct.ShouldThrow<ArgumentOutOfRangeException, int>(-1, "a negative Capacity is invalid");
            construct.ShouldThrow<ArgumentOutOfRangeException, int>(0, "a 0 Capacity is invalid");
        }
        #endregion Construction

        #region Test_Add
        [TestMethod]
        public void Test_Add()
        {
            // First try overwrite defaulted to true
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 0;
            var cb = new CircularBuffer<int>(capacity, overwrite);
            cb.Count.Should().Be(0, $"Count should start off at 0");

            cb.Add(1);
            ++expectedCount;
            cb.Count.Should().Be(expectedCount);

            expectedCount = 5;
            Enumerable.Range(2, expectedCount - 1).ForEach(i => cb.Add(i));
            cb.Count.Should().Be(expectedCount);
            cb.Should().Equal(1, 2, 3, 4, 5);

            // Overwrite should be allowed
            cb.Add(6);
            cb.Count.Should().Be(expectedCount, "Count should not have exceeded the Capacity");
            cb.Should().Equal(2, 3, 4, 5, 6);

            Action<int> add1 = i => cb.Add(i);
            Action<int, bool> add2 = (i, o) => cb.Add(i, o);

            // Overwrite should not be allowed
            add2.ShouldThrow<InvalidOperationException, int, bool>(7, false);
            cb.Count.Should().Be(expectedCount, "Count should not have increased");
            cb.Should().Equal(new[] { 2, 3, 4, 5, 6 }, "the collection should not have changed");

            // Now try with overwrite defaulted to false
            overwrite = false;
            cb = new CircularBuffer<int>(capacity, overwrite);
            expectedCount = 5;

            Enumerable.Range(1, expectedCount).ForEach(i => cb.Add(i));
            cb.Count.Should().Be(expectedCount);
            cb.Should().Equal(1, 2, 3, 4, 5);

            // Overwrite should not be allowed
            add1.ShouldThrow<InvalidOperationException, int>(6);
            cb.Count.Should().Be(expectedCount, "Count should not have increased");
            cb.Should().Equal(new[] { 1, 2, 3, 4, 5 }, "the collection should not have changed");

            // Overwrite should be allowed
            cb.Add(7, true);
            cb.Count.Should().Be(expectedCount, "Count should not have increased");
            cb.Should().Equal(new[] { 2, 3, 4, 5, 7 }, "the collection should not have changed");

            // Set overwrite manually
            cb.Overwrite = true;
            cb.Add(8);
            cb.Count.Should().Be(expectedCount, "Count should not have increased");
            cb.Should().Equal(new[] { 3, 4, 5, 7, 8 }, "the collection should not have changed");
        }
        #endregion Test_Add

        #region Test_Contains
        [TestMethod]
        public void Test_Contains()
        {
            var capacity = 5;
            var count = capacity;
            var overwrite = true;

            // Test with value types
            {
                var cb = CreatePopulated(count, capacity, overwrite);
                var itemToFind = count / 2;
                cb.Contains(itemToFind).Should().BeTrue($"{itemToFind} is expected to be in the collection");
                itemToFind = count + 2;
                cb.Contains(itemToFind).Should().BeFalse($"{itemToFind} is NOT expected to be in the collection");
            }

            // Repeat the test with a reference type
            {
                var cb = CreatePopulated<RefType>(count, i => new RefType() { Data = i.ToString() }, capacity, overwrite);
                var itemToFind = cb[capacity / 2];
                cb.Contains(itemToFind).Should().BeTrue($"{itemToFind} is expected to be in the collection");
                itemToFind = new RefType() { Data = (count / 2).ToString() };
                cb.Contains(itemToFind).Should().BeFalse($"{itemToFind} is NOT expected to be in the collection");
            }

        }
        #endregion Test_Contains

        #region Test_Remove
        [TestMethod]
        public void Test_Remove()
        {
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 5;
            var cb = CreatePopulated(expectedCount, capacity, overwrite);

            // Test the ICollection implementation.
            Action remove = () => ((ICollection<int>)cb).Remove(2);
            remove.ShouldThrow<NotSupportedException>();

            // Test the regular implementation
            var item = cb.Remove();
            item.Should().Be(1, "item should be the first element");
            cb.Count.Should().Be(expectedCount - 1, "Count should have decreased by 1");
            cb.Should().Equal(2, 3, 4, 5);

            for(var i = 0; i < expectedCount - 2; ++i)
                cb.Remove();
            item = cb.Remove();

            item.Should().Be(5, "item should be the last element");
            cb.Count.Should().Be(0, "the buffer should be empty");
            cb.Should().Equal(new int[] { });

            // Test exceptions
            remove = () => cb.Remove();
            remove.ShouldThrow<InvalidOperationException>("you shouldn't be able to remove from an empty buffer");
        }
        #endregion Test_Remove

        #region Test_Indexing
        [TestMethod]
        public void Test_Indexing()
        {
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 5;
            var cb = CreatePopulated(expectedCount, capacity, overwrite);

            for (var i = 0; i < expectedCount; ++i)
                cb[i].Should().Be(i + 1, "indexed items appear in the order they were added");

            cb.Add(expectedCount + 1);
            for (var i = 0; i < expectedCount; ++i)
                cb[i].Should().Be(i + 2, "indexing should reflect the newly added item");

            cb.Remove();
            for (var i = 0; i < expectedCount - 1; ++i)
                cb[i].Should().Be(i + 3, "indexing should reflect the newly removed item");

            // Test exceptions
            int iTemp;
            Action<int> indexing = i => iTemp = cb[i];
            indexing.ShouldThrow<ArgumentOutOfRangeException, int>(-1, "negatives not allowed");
            indexing.ShouldThrow<ArgumentOutOfRangeException, int>(capacity, "index must be in range");
        }
        #endregion Test_Indexing

        #region Test_ReadOnly
        [TestMethod]
        public void Test_ReadOnly()
        {
            var cb = new CircularBuffer<int>();
            cb.IsReadOnly.Should().BeFalse("ReadOnly should always be false");
        }
        #endregion Test_ReadOnly

        #region Test_Clear
        [TestMethod]
        public void Test_Clear()
        {
            var capacity = 5;
            var overwrite = true;

            Action<int> testClear = size =>
            {
                var expectedSize = size > capacity ? capacity : size;
                var cb = CreatePopulated(expectedSize, capacity, overwrite);
                cb.Clear();
                cb.Count.Should().Be(0);
            };

            // Test on an empty buffer
            testClear(0);

            // Test on a partially filled buffer
            testClear(capacity / 2);

            // Test on a full buffer
            testClear(capacity);

            // Test on an over-full buffer
            testClear(capacity + (capacity / 2));
        }
        #endregion Test_Clear

        #region Test_CopyTo
        [TestMethod]
        public void Test_CopyTo()
        {
            var offset = 0;
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 4;
            var cb = CreatePopulated(expectedCount, capacity, overwrite);

            // Basic CopyTo check
            var data = new int[capacity];
            cb.CopyTo(data, offset);
            cb.Count.Should().Be(expectedCount, "Count should not change after a copy");
            cb.Should().Equal(new[] { 1, 2, 3, 4 }, "elements should not have changed");
            cb.Should().Equal(data.Skip(offset).Take(expectedCount), "the buffer should match the array");

            // CopyTo with offset
            offset = 1;
            cb.CopyTo(data, offset);
            cb.Count.Should().Be(expectedCount, "Count should not change after a copy");
            cb.Should().Equal(new[] { 1, 2, 3, 4 }, "elements should not have changed");
            cb.Should().Equal(data.Skip(offset).Take(expectedCount), "the buffer should match the array");

            // CopyTo with length
            var length = 3;
            offset = 1;
            cb.CopyTo(data, offset, length);
            cb.Count.Should().Be(expectedCount, "Count should not change after a copy");
            cb.Should().Equal(new[] { 1, 2, 3, 4 }, "elements should not have changed");
            cb.Should().Equal(data.Skip(offset).Take(expectedCount), "the buffer should match the array");

            // CopyTo with Remove()
            cb.Remove();
            --expectedCount;
            cb.CopyTo(data, offset);
            cb.Count.Should().Be(expectedCount, "Count should not change after a copy");
            cb.Should().Equal(new[] { 2, 3, 4 }, "elements should not have changed");
            cb.Should().Equal(data.Skip(offset).Take(expectedCount), "the buffer should match the array");

            // CopyTo with Add()
            cb.Add(5);
            ++expectedCount;
            cb.CopyTo(data, offset);
            cb.Count.Should().Be(expectedCount, "Count should not change after a copy");
            cb.Should().Equal(new[] { 2, 3, 4, 5 }, "elements should not have changed");
            cb.Should().Equal(data.Skip(offset).Take(expectedCount), "the buffer should match the array");

            // Test exception
            Action<int[], int, int> copy = (d, o, l) => cb.CopyTo(d, o, l);
            Action<int[], int> copy2 = (d, o) => cb.CopyTo(d, o);

            copy.ShouldThrow<ArgumentNullException, int[], int, int>(null, offset, length, "the array can't be null");
            copy.ShouldThrow<ArgumentOutOfRangeException, int[], int, int>(data, -1, length, "offset can't be negative");
            copy.ShouldThrow<ArgumentOutOfRangeException, int[], int, int>(data, offset, -1, "length can't be negative");

            copy.ShouldThrow<InvalidOperationException, int[], int, int>(new int[2], 0, 3, "the array must be big enough for offset and length");
            copy2.ShouldThrow<InvalidOperationException, int[], int>(new int[2], 4, "the array must be big enough for offset and length");
        }
        #endregion Test_CopyTo

        #region Test_Enumeration
        [TestMethod]
        public void Test_Enumeration()
        {
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 4;

            // Buffer set up
            var cb = CreatePopulated(expectedCount, capacity, overwrite);
            var items = string.Join(".", cb.Select(i => i.ToString())); // Join them up to test enumeration
            items.Should().Be("1.2.3.4");

            var en = cb.GetEnumerator();
            en.MoveNext();
            en.Current.Should().Be(1);
            en.MoveNext();
            en.Current.Should().Be(2);
            en.MoveNext();
            en.Current.Should().Be(3);
            en.MoveNext();
            en.Current.Should().Be(4);

        }
        #endregion Test_Enumeration

        #region Test_ToArray
        [TestMethod]
        public void Test_ToArray()
        {
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 5;

            // Buffer set up
            var cb = CreatePopulated(expectedCount, capacity, overwrite);
            cb.ToArray().Should().Equal(1,2,3,4,5);
            cb.ToArray().Should().Equal(cb);

            cb.Remove();
            cb.Add(6);
            cb.ToArray().Should().Equal(2, 3, 4, 5, 6);
            cb.ToArray().Should().Equal(cb);

        }
        #endregion Test_Enumeration

        [TestMethod]
        public void Test_Read()
        {
            var capacity = 30;
            var overwrite = true;
            var expectedCount = 10;
            var offset = 0;
            var toRead = 5;

            // Buffer set up
            var cb = CreatePopulated(expectedCount, capacity, overwrite);
            var data = new int[expectedCount];
            var itemsRead = cb.Read(data, offset, toRead);

            itemsRead.Should().Be(toRead);
            cb.Count.Should().Be(expectedCount - toRead);

        }

        // Read - incomplete
        // Write

        class RefType
        {
            public string Data;
        }

        static CircularBuffer<int> CreatePopulated(int count, int capacity = CircularBuffer<int>.DEFAULT_CAPACITY, bool overwrite = CircularBuffer<int>.DEFAULT_OVERWRITE)
            => CreatePopulated<int>(count, i => i, capacity, overwrite);

        static CircularBuffer<T> CreatePopulated<T>(int count, Func<int, T> itemCreator, int capacity = CircularBuffer<T>.DEFAULT_CAPACITY, bool overwrite = CircularBuffer<T>.DEFAULT_OVERWRITE)
        {
            var checkList = new List<T>();
            var cb = new CircularBuffer<T>(capacity, overwrite);
            Enumerable.Range(1, count)
                .Select(i => itemCreator(i))
                .ForEach(i => { cb.Add(i); checkList.Add(i); });
            cb.Count.Should().Be(count > capacity ? capacity : count);
            cb.Should().Equal(checkList);
            return cb;
        }
    }
}
