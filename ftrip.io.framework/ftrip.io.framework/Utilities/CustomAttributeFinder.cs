using System;
using System.Collections.Generic;
using System.Linq;

namespace ftrip.io.framework.Utilities
{
    public static class CustomAttributeFinder
    {
        public static IList<TAttribute> GetFrom<TAttribute>(Type type)
        {
            var mappableAttributes = type.GetCustomAttributes(typeof(TAttribute), true);

            return mappableAttributes.Cast<TAttribute>().ToList();
        }

        public static bool Has<TAttribute>(Type type) => GetFrom<TAttribute>(type).Any();
    }
}