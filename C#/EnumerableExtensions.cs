using System;
using System.Collections.Generic;
using System.Text;

namespace HisRoyalRedness.com
{
    internal static class EnumerableExtensions
    {
        internal static T Tap<T>(this T item, Action<T> action)
        {
            action(item);
            return item;
        }
    }
}
