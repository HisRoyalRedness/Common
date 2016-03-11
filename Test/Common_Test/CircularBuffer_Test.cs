using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fletcher.org;

namespace fletcher.org.Tests
{
    [TestClass]
    public class CircularBuffer_Test
    {
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
        }

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

            cb.Add(2);
            cb.Add(3);
            cb.Add(4);
            cb.Add(5);
            expectedCount = 5;
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

            cb.Add(1);
            cb.Add(2);
            cb.Add(3);
            cb.Add(4);
            cb.Add(5);
            expectedCount = 5;
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

        [TestMethod]
        public void Test_Misc()
        {
            var cb = new CircularBuffer<int>();
            Assert.AreEqual(false, cb.IsReadOnly, $"ReadOnly should always be false.");
        }
    }

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
