using System.Collections.Generic;

namespace Grimity.Collections {
public static class ICollectionExtensions {
    public static bool IsEmpty<T>(this ICollection<T> list) {
        return list.Count == 0;
    }
}
}