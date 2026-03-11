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

    [Obsolete("IEnumerable is not good for shuffle, use array or list specific functions")]
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list) => list.OrderBy(_ => Random.value);

    public static T[] Shuffle<T>(this T[] source) {
        var array = (T[])source.Clone();
        ShuffleInPlace(array);
        return array;
    }

    public static T[] ShuffleInPlace<T>(this T[] array) {
        for (var i = array.Length - 1; i > 0; i--) {
            var j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }

        return array;
    }

    public static List<T> Shuffle<T>(this List<T> source) {
        var list = new List<T>(source);
        ShuffleInPlace(list);
        return list;
    }

    public static List<T> ShuffleInPlace<T>(this List<T> list) {
        for (var i = list.Count - 1; i > 0; i--) {
            var j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        return list;
    }
}
}