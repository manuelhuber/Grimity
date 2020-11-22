using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grimity.Collections {
public static class EnumerableExtensions {
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable) {
        return new HashSet<T>(enumerable);
    }

    public static T GetRandomElement<T>(this IEnumerable<T> list) {
        var enumerable = list as T[] ?? list.ToArray();
        var index = Random.Range(0, enumerable.Count());
        return enumerable.ElementAt(index);
    }
}
}