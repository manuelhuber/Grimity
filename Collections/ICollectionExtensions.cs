using System.Collections.Generic;

namespace Grimity.Collections {
public static class ICollectionExtensions {
    public static bool IsEmpty<T>(this ICollection<T> list) {
        return list.Count == 0;
    }

    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> range) {
        foreach (var obj in range) collection.Add(obj);
    }
}
}