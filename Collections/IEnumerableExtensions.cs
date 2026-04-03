using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

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

    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action) {
        foreach (var obj in ie) {
            action(obj);
        }
    }

    public static IEnumerable<T> NotNull<T>(this IEnumerable<T> enumerable) {
        return enumerable.Where(i => i != null);
    }

    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count, bool throwIfNotEnough = false) {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "count must be ≥ 0");

        var list = source.ToList();

        if (count > list.Count) {
            if (throwIfNotEnough) {
                throw new ArgumentOutOfRangeException(nameof(count),
                    $"Requested {count} elements but source only has {list.Count}.");
            }

            count = list.Count;
        }

        for (var i = 0; i < count; i++) {
            var j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }

        return list.Take(count);
    }
}
}