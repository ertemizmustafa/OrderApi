using System.Collections.Generic;
using System.Linq;

namespace Order.Logic.Common
{
    public static class CommonExtensions
    {
        public static bool ContainItem<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }
    }
}
