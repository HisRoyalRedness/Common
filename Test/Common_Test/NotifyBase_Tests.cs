using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HisRoyalRedness.com
{
    [TestClass]
    public class NotifyBase_Tests
    {
        [TestMethod]
        public void Basic_change_notification()
        {
            //var mock = new NotifyTest();
            //mock.MonitorEvents<INotifyPropertyChanged>();
            //mock.ShouldRaisePropertyChangeFor(x => x.StringValue);
        }
    }

    class NotifyTest : NotifyBase
    {
        public string StringValue
        {
            get { return _stringValue; }
            set { SetProperty(nameof(StringValue), value, ref _stringValue); }
        }
        string _stringValue = "123";

        public int IntValue
        {
            get { return _intValue; }
            set { SetProperty(nameof(IntValue), value, ref _intValue); }
        }
        int _intValue;
    }
}
