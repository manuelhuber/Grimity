using System;
using System.Collections.Generic;

namespace Grimity.Collections {
public static class IReadOnlyListExtensions {
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item) {
        for (var i = 0; i < list.Count; i++) {
            if (list[i]?.Equals(item) ?? false) {
                return i;
            }
        }

        return -1;
    }

    public static List<int> GetIndices<T>(this IReadOnlyList<T> list, Predicate<T> predicate) {
        var indices = new List<int>();
        for (var i = 0; i < list.Count; i++) {
            if (predicate(list[i])) indices.Add(i);
        }

        return indices;
    }
}
}