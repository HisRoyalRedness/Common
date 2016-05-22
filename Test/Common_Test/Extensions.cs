using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace HisRoyalRedness.com.Tests
{
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
        public static FluentAssertions.Specialized.ExceptionAssertions<TException> ShouldThrow<TException, TValue>(this Action<TValue> action, TValue value, string because = "", params object[] reasonArgs)
            where TException : Exception
        {
            Action simpleAction = () => action(value);
            return simpleAction.ShouldThrow<TException>(because, reasonArgs);
        }

        public static FluentAssertions.Specialized.ExceptionAssertions<TException> ShouldThrow<TException, TValue1, TValue2>(this Action<TValue1, TValue2> action, TValue1 value1, TValue2 value2, string because = "", params object[] reasonArgs)
            where TException : Exception
        {
            Action simpleAction = () => action(value1, value2);
            return simpleAction.ShouldThrow<TException>(because, reasonArgs);
        }

        public static FluentAssertions.Specialized.ExceptionAssertions<TException> ShouldThrow<TException, TValue1, TValue2, TValue3>(this Action<TValue1, TValue2, TValue3> action, TValue1 value1, TValue2 value2, TValue3 value3, string because = "", params object[] reasonArgs)
            where TException : Exception
        {
            Action simpleAction = () => action(value1, value2, value3);
            return simpleAction.ShouldThrow<TException>(because, reasonArgs);
        }
    }
    #endregion Test Extensions
}
