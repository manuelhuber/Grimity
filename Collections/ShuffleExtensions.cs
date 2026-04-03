using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grimity.Collections {
public static class ShuffleExtensions {
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list) => list.OrderBy(_ => Random.value);

    public static T[] Shuffle<T>(this T[] source) {
        var array = (T[])source.Clone();
        array.ShuffleInPlace();
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
        list.ShuffleInPlace();
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