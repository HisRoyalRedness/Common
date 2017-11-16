using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Threading;

namespace HisRoyalRedness.com.Tests
{
    [TestClass]
    public class NotifyBase_GetAndSet_Test
    {
        [TestMethod]
        public void GetProperty_without_a_lock_object()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = "a quick brown fox";

            // Act
            // Assert
            notifier.TestProperty.Should().BeSameAs("a quick brown fox");
        }

        [TestMethod]
        public void GetProperty_with_a_lock_object()
        {
            // Arrange
            var lockObject = new object();
            var notifier = new Notifier(lockObject);
            notifier.TestPropertyDirect = "a quick brown fox";

            // Act
            // Assert
            notifier.LockObject.Should().BeSameAs(lockObject);
            notifier.TestProperty.Should().Be("a quick brown fox");
        }

        [TestMethod]
        public void GetProperty_with_locking()
        {
            // Arrange
            var lockObject = new object();
            var notifier = new Notifier(lockObject);
            notifier.TestPropertyDirect = "a quick brown fox";

            var getValue = "";
            var getValueLock = new object();

            var rePreUpdate = new ManualResetEventSlim();
            var rePostUpdate = new ManualResetEventSlim();
            var updateThread = new Thread(() =>
            {
                rePreUpdate.Set();
                var value = notifier.TestProperty;
                lock (getValueLock)
                    getValue = value;
                rePostUpdate.Set();
            });

            // Act
            // Assert

            // Hold the lock on this thread
            lock (lockObject)
            {
                // Start another thread that tries to take the lock
                updateThread.Start();
                // Make sure that the other thread is up and running
                rePreUpdate.Wait();
                // An arbitrary delay to make sure the other thread is blocked
                Thread.Sleep(1000);
                // Make sure that value hasn't been updated
                lock (getValueLock)
                    getValue.Should().Be("");
            }

            // We're out the lock now, so the value should be fetched. 
            // Make sure the other thread has finished getting the value
            rePostUpdate.Wait();
            lock (getValueLock)
                getValue.Should().Be("a quick brown fox");
        }

        [TestMethod]
        public void SetProperty_without_a_lock_object()
        {
            // Arrange
            var notifier = new Notifier();

            // Act
            // Assert
            notifier.TestProperty = "a quick brown fox";
            notifier.TestPropertyDirect.Should().BeSameAs("a quick brown fox");
        }

        [TestMethod]
        public void SetProperty_with_a_lock_object()
        {
            // Arrange
            var lockObject = new object();
            var notifier = new Notifier(lockObject);

            // Act
            // Assert
            notifier.LockObject.Should().BeSameAs(lockObject);
            notifier.TestProperty = "a quick brown fox";
            notifier.TestPropertyDirect.Should().BeSameAs("a quick brown fox");
        }

        [TestMethod]
        public void SetProperty_with_locking()
        {
            // Arrange
            var lockObject = new object();
            var notifier = new Notifier(lockObject);


            var rePreUpdate = new ManualResetEventSlim();
            var rePostUpdate = new ManualResetEventSlim();
            var updateThread = new Thread(() =>
            {
                rePreUpdate.Set();
                notifier.TestProperty = "a quick brown fox";
                rePostUpdate.Set();
            });

            // Act
            // Assert
            lock (lockObject)
            {
                notifier.TestProperty = "123";
                notifier.TestPropertyDirect.Should().BeSameAs("123", "this thread holds the lock, so it can update the value");

                // Start another thread that tries to take the lock
                updateThread.Start();
                // Make sure that the other thread is up and running
                rePreUpdate.Wait();
                // An arbitrary delay to make sure the other thread is blocked
                Thread.Sleep(1000);
                // Make sure that value hasn't been updated
                notifier.TestProperty.Should().BeSameAs("123", "this thread holds the lock, so it can fetch the value");
            }

            // We're out the lock now, so the value should be updated. 
            // Make sure the other thread has finished getting the value
            rePostUpdate.Wait();
            notifier.TestProperty.Should().BeSameAs("a quick brown fox");
        }
    }

    [TestClass]
    public class NotifyBase_ChangeDetection_Test
    {
        [TestMethod]
        public void SetProperty_null_to_null_shouldnt_change()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = null;
            notifier.TestProperty.Should().Be(null);

            // Act
            var result = notifier.SetTestProperty(null);

            // Assert
            result.Should().BeFalse();
            notifier.TestProperty.Should().Be(null);
        }

        [TestMethod]
        public void SetProperty_null_to_something_should_change()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = null;
            notifier.TestProperty.Should().Be(null);

            // Act
            var result = notifier.SetTestProperty("blah");

            // Assert
            result.Should().BeTrue();
            notifier.TestProperty.Should().Be("blah");
        }

        [TestMethod]
        public void SetProperty_something_to_something_shouldnt_change()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = "blah";
            notifier.TestProperty.Should().Be("blah");

            // Act
            var result = notifier.SetTestProperty("blah");

            // Assert
            result.Should().BeFalse();
            notifier.TestProperty.Should().Be("blah");
        }

        [TestMethod]
        public void SetProperty_something_to_something_else_should_change()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = "blah";
            notifier.TestProperty.Should().Be("blah");

            // Act
            var result = notifier.SetTestProperty("blah new");

            // Assert
            result.Should().BeTrue();
            notifier.TestProperty.Should().Be("blah new");
        }

        [TestMethod]
        public void SetProperty_something_to_null_should_change()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = "blah";
            notifier.TestProperty.Should().Be("blah");

            // Act
            var result = notifier.SetTestProperty(null);

            // Assert
            result.Should().BeTrue();
            notifier.TestProperty.Should().Be(null);
        }
    }

    [TestClass]
    public class NotifyBase_ChangeNotification
    {
        [TestMethod]
        public void SetProperty_no_change_shouldnt_notify()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = null;
            notifier.TestProperty.Should().Be(null);

            // Act
            var result = notifier.SetTestProperty(null);

            // Assert
            result.Should().BeFalse();
            notifier.PropertyChangedActionFired.Should().BeFalse();
            notifier.PropertyChangedEventFired.Should().BeFalse();
            notifier.TestProperty.Should().Be(null);
        }

        [TestMethod]
        public void SetProperty_change_should_notify()
        {
            // Arrange
            var notifier = new Notifier();
            notifier.TestPropertyDirect = null;
            notifier.TestProperty.Should().Be(null);

            // Act
            var result = notifier.SetTestProperty("blah");

            // Assert
            result.Should().BeTrue();
            notifier.PropertyChangedActionFired.Should().BeTrue();
            notifier.PropertyChangedEventFired.Should().BeTrue();
            notifier.TestProperty.Should().Be("blah");
        }
    }

    class Notifier : NotifyBase<object>
    {
        public Notifier()
            : base()
        {
            HookChangeNotification();
        }

        public Notifier(object lockObject)
            : base(lockObject)
        {
            HookChangeNotification();
        }

        void HookChangeNotification()
        {
            _changeAction = new Action<string>(nv => _changeActionFired = true);
            PropertyChanged += (o, e) => _changeEventFired = (e.PropertyName == nameof(TestProperty));
        }

        private void Notifier_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public object LockObject => _propertyLock;


        public string TestProperty
        {
            get { return GetProperty(ref _testProperty); }
            set { SetProperty(ref _testProperty, value, _changeAction); }
        }

        public string TestPropertyDirect
        {
            get { return _testProperty; }
            set { _testProperty = value; }
        }

        public bool SetTestProperty(string newValue, Action<string> changeAction = null)
            => SetProperty(ref _testProperty, newValue, nv =>
            {
                if (changeAction != null)
                    changeAction(nv);
                if (_changeAction != null)
                    _changeAction(nv);
            }, nameof(TestProperty));

        public bool PropertyChangedActionFired => _changeActionFired;
        public bool PropertyChangedEventFired => _changeEventFired;


        Action<string> _changeAction;
        string _testProperty = null;
        bool _changeActionFired = false;
        bool _changeEventFired = false;

    }
}
