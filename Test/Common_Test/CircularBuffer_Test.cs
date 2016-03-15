using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fletcher.org;
using System.Collections.Generic;

namespace fletcher.org.Tests
{
    [TestClass]
    public class CircularBuffer_Test
    {
        #region TestConstruction
        [TestMethod]
        public void TestConstruction()
        {
            var capacity = 1024;
            var overwrite = false;
            var cb = new CircularBuffer<int>();
            Assert.AreEqual(capacity, cb.Capacity, $"Default Capacity should be {capacity}.");
            Assert.AreEqual(overwrite, cb.Overwrite, $"Default Overwrite should be {overwrite}.");

            capacity = 100;
            cb = new CircularBuffer<int>(capacity);
            Assert.AreEqual(capacity, cb.Capacity, "Specified Capacity should be {capacity}.");
            Assert.AreEqual(overwrite, cb.Overwrite, $"Default Overwrite should be {overwrite}.");

            capacity = 200;
            overwrite = true;
            cb = new CircularBuffer<int>(capacity, overwrite);
            Assert.AreEqual(capacity, cb.Capacity, "Specified Capacity should be {capacity}.");
            Assert.AreEqual(overwrite, cb.Overwrite, $"Specified Overwrite should be {overwrite}.");

            // Test Exceptions
            AssertExt.MustThrow<ArgumentOutOfRangeException>(() => new CircularBuffer<int>(-1));
            AssertExt.MustThrow<ArgumentOutOfRangeException>(() => new CircularBuffer<int>(0));
        }
        #endregion TestConstruction

        #region Test_Add
        [TestMethod]
        public void Test_Add()
        {
            // First try overwrite defaulted to true
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 0;
            var cb = new CircularBuffer<int>(capacity, overwrite);
            Assert.AreEqual(0, cb.Count, $"Count should start off at 0.");

            cb.Add(1);
            expectedCount = 1;
            Assert.AreEqual(expectedCount, cb.Count, $"Count should be {expectedCount}.");

            expectedCount = 5;
            Enumerable.Range(2, expectedCount - 1).ForEach(i => cb.Add(i));
            Assert.AreEqual(expectedCount, cb.Count, $"Count should be {expectedCount}.");
            var items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("1.2.3.4.5", items, "Checking the elements in the buffer");

            // Overwrite should be allowed
            cb.Add(6);
            Assert.AreEqual(expectedCount, cb.Count, $"Count should not have increased from {expectedCount}.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("2.3.4.5.6", items, "Checking the elements in the buffer");

            // Overwrite should not be allowed
            AssertExt.MustThrow<InvalidOperationException>(() => cb.Add(7, false));
            Assert.AreEqual(expectedCount, cb.Count, $"Count should not have increased from {expectedCount}.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("2.3.4.5.6", items, "Checking the elements in the buffer");

            // Now try with overwrite defaulted to false
            overwrite = false;
            cb = new CircularBuffer<int>(capacity, overwrite);

            expectedCount = 5;
            Enumerable.Range(1, expectedCount).ForEach(i => cb.Add(i));
            Assert.AreEqual(expectedCount, cb.Count, $"Count should be {expectedCount}.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("1.2.3.4.5", items, "Checking the elements in the buffer");

            // Overwrite should not be allowed
            AssertExt.MustThrow<InvalidOperationException>(() => cb.Add(6));
            Assert.AreEqual(expectedCount, cb.Count, $"Count should not have increased from {expectedCount}.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("1.2.3.4.5", items, "Checking the elements in the buffer");

            // Overwrite should be allowed
            cb.Add(7, true);
            Assert.AreEqual(expectedCount, cb.Count, $"Count should not have increased from {expectedCount}.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("2.3.4.5.7", items, "Checking the elements in the buffer");

            cb.Overwrite = true;
            cb.Add(8);
            Assert.AreEqual(expectedCount, cb.Count, $"Count should not have increased from {expectedCount}.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("3.4.5.7.8", items, "Checking the elements in the buffer");
        }
        #endregion Test_Add

        #region Test_Contains
        [TestMethod]
        public void Test_Contains()
        {
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 5;
            var cb = new CircularBuffer<int>(capacity, overwrite);

            // Test with value types
            {
                Enumerable.Range(1, expectedCount).ForEach(i => cb.Add(i));
                Assert.AreEqual(expectedCount, cb.Count, $"Count should be {expectedCount}.");
                var items = string.Join(".", cb.Select(i => i.ToString()));
                Assert.AreEqual("1.2.3.4.5", items, "Checking the elements in the buffer");

                var itemToFind = 2;
                Assert.IsTrue(cb.Contains(itemToFind), $"Should find {itemToFind} in the buffer.");
                itemToFind = 22;
                Assert.IsFalse(cb.Contains(itemToFind), $"Should NOT find {itemToFind} in the buffer.");
            }

            // Repeat the test with a reference type
            {
                var cbr = new CircularBuffer<RefType>(capacity, overwrite);

                Enumerable.Range(1, expectedCount).ForEach(i => cbr.Add(new RefType() { Data = i.ToString() }));
                Assert.AreEqual(expectedCount, cbr.Count, $"Count should be {expectedCount}.");

                var itemToFind = cbr[1];
                Assert.IsTrue(cbr.Contains(itemToFind), $"Should find {itemToFind} in the buffer.");
                itemToFind = new RefType() { Data = "2" };
                Assert.IsFalse(cbr.Contains(itemToFind), $"Should NOT find {itemToFind} in the buffer.");
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
            var cb = new CircularBuffer<int>(capacity, overwrite);
            Enumerable.Range(1, expectedCount).ForEach(i => cb.Add(i));

            // Test the ICollection implementation
            AssertExt.MustThrow<NotSupportedException>(() => ((ICollection<int>)cb).Remove(2), "Remove() shouldn't be supported.");

            var item = cb.Remove();
            Assert.AreEqual(1, item, "Should have removed the first item");
            Assert.AreEqual(expectedCount - 1, cb.Count, "Count should have decremented by 1.");
            var items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("2.3.4.5", items, "Checking the elements in the buffer");

            for(var i = 0; i < expectedCount - 2; ++i)
                cb.Remove();
            item = cb.Remove();
            Assert.AreEqual(5, item, "Should have removed the last item");
            Assert.AreEqual(0, cb.Count, "Count should be 0.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("", items, "Checking the elements in the buffer");

            // Test exceptions
            AssertExt.MustThrow<InvalidOperationException>(() => cb.Remove(), "Shouldn't be able to Remove from an empty buffer.");
        }
        #endregion Test_Remove

        #region Test_Indexing
        [TestMethod]
        public void Test_Indexing()
        {
            var capacity = 5;
            var overwrite = true;
            var expectedCount = 5;
            var cb = new CircularBuffer<int>(capacity, overwrite);
            Enumerable.Range(1, expectedCount).ForEach(i => cb.Add(i));

            for (var i = 0; i < expectedCount; ++i)
                Assert.AreEqual(i + 1, cb[i], "Indexed items should match");

            cb.Add(expectedCount + 1);
            for (var i = 0; i < expectedCount; ++i)
                Assert.AreEqual(i + 2, cb[i], "Index still works after Add");

            cb.Remove();
            for (var i = 0; i < expectedCount - 1; ++i)
                Assert.AreEqual(i + 3, cb[i], "Index still works after Remove");

            // Test exceptions
            int ii;
            AssertExt.MustThrow<ArgumentOutOfRangeException>(() => ii = cb[-1], "No negative indexes");
            AssertExt.MustThrow<ArgumentOutOfRangeException>(() => ii = cb[capacity], "No out-of-range");
        }
        #endregion Test_Indexing

        #region Test_ReadOnly
        [TestMethod]
        public void Test_ReadOnly()
        {
            var cb = new CircularBuffer<int>();
            Assert.AreEqual(false, cb.IsReadOnly, $"ReadOnly should always be false.");
        }
        #endregion Test_ReadOnly

        #region Test_Clear
        [TestMethod]
        public void Test_Clear()
        {
            var capacity = 5;

            Action<int> testClear = size =>
            {
                var expectedSize = size > capacity ? capacity : size;
                var cb = new CircularBuffer<int>(capacity, true);
                Enumerable.Range(1, expectedSize).ForEach(i => cb.Add(i));
                Assert.AreEqual(expectedSize, cb.Count, $"Count should be {expectedSize}.");
                cb.Clear();
                Assert.AreEqual(0, cb.Count, $"Count should be 0.");
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

            // Buffer set up
            var cb = new CircularBuffer<int>(capacity, overwrite);
            Enumerable.Range(1, expectedCount).ForEach(i => cb.Add(i));
            var items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual(expectedCount, cb.Count, $"Count should be {expectedCount}.");
            Assert.AreEqual("1.2.3.4", items, "Checking the elements in the buffer");

            // Basic CopyTo check
            var data = new int[capacity];
            cb.CopyTo(data, offset);
            Assert.AreEqual(expectedCount, cb.Count, $"Count should not have changed from {expectedCount}.");
            items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("1.2.3.4", items, "Elements should not have changed");
            items = string.Join(".", data.Skip(offset).Take(expectedCount).Select(i => i.ToString()));
            Assert.AreEqual("1.2.3.4", items, "Array should match buffer");

            // CopyTo with offset
            offset = 1;
            cb.CopyTo(data, offset);
            items = string.Join(".", data.Skip(offset).Take(expectedCount).Select(i => i.ToString()));
            Assert.AreEqual("1.2.3.4", items, "Array should match buffer");

            // CopyTo with length
            var length = 3;
            offset = 1;
            cb.CopyTo(data, offset, length);
            items = string.Join(".", data.Skip(offset).Take(length).Select(i => i.ToString()));
            Assert.AreEqual("1.2.3", items, "Array should match buffer");

            // CopyTo with Remove()
            cb.Remove();
            --expectedCount;
            cb.CopyTo(data, offset);
            items = string.Join(".", data.Skip(offset).Take(expectedCount).Select(i => i.ToString()));
            Assert.AreEqual("2.3.4", items, "Array should match buffer");

            // CopyTo with Add()
            cb.Add(5);
            ++expectedCount;
            cb.CopyTo(data, offset);
            items = string.Join(".", data.Skip(offset).Take(expectedCount).Select(i => i.ToString()));
            Assert.AreEqual("2.3.4.5", items, "Array should match buffer");

            // Test exception
            AssertExt.MustThrow<ArgumentNullException>(() => cb.CopyTo(null, offset, length));
            AssertExt.MustThrow<ArgumentOutOfRangeException>(() => cb.CopyTo(data, -1, length));
            AssertExt.MustThrow<ArgumentOutOfRangeException>(() => cb.CopyTo(data, offset, -1));

            data = new int[2];
            AssertExt.MustThrow<InvalidOperationException>(() => cb.CopyTo(data, 0, 3));
            AssertExt.MustThrow<InvalidOperationException>(() => cb.CopyTo(data, 4));
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
            var cb = new CircularBuffer<int>(capacity, overwrite);
            Enumerable.Range(1, expectedCount).ForEach(i => cb.Add(i));
            Assert.AreEqual(expectedCount, cb.Count, $"Count should be {expectedCount}.");
            var items = string.Join(".", cb.Select(i => i.ToString()));
            Assert.AreEqual("1.2.3.4", items, "Checking the elements in the buffer");
        }
        #endregion Test_Enumeration

        // Read
        // Write

        class RefType
        {
            public string Data;
        }
    }

    #region IEnumerableExtensions
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
                action(item);
        }
    }
    #endregion IEnumerableExtensions

    #region Test Extensions
    public static class AssertExt
    {
        public static void MustThrow<TException>(Action action, string message = "")
            where TException : Exception
        {
            try
            {
                action();
                Assert.Fail($"Expected a {typeof(TException).Name} to be thrown.");
            }
            catch(TException)
            {
                // All good!
            }
            catch(Exception ex)
            {
                Assert.Fail($"Caught an unexpected {ex.GetType().Name}. {ex}");
            }
        }
    }
    #endregion Test Extensions
}
