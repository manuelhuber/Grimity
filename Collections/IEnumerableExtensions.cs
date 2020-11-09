using System.Collections.Generic;

namespace Grimity.Collections {
public static class EnumerableExtensions {
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable) {
        return new HashSet<T>(enumerable);
    }
}
}