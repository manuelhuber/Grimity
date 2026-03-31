using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Grimity.Collections {
public static class CollectionExtensions {
    public static T GetRandomElement<T>(this IList<T> list) {
        return list[Random.Range(0, list.Count)];
    }

    public static T GetRandomElement<T>(this T[] array) {
        if (array.Length == 0) return default;
        return array[Random.Range(0, array.Length)];
    }

    public static T Last<T>(this Collection<T> list) {
        return list[list.Count - 1];
    }

    public static T Dequeue<T>(this Collection<T> list) {
        var t = list.First();
        list.Remove(t);
        return t;
    }

    public static List<T> Dequeue<T>(this Collection<T> list, int count) {
        var t = list.Take(count).ToList();
        foreach (var item in t) {
            list.Remove(item);
        }

        return t;
    }
}
}