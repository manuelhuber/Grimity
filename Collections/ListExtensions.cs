using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Grimity.Collections {
public static class CollectionExtensions {
    public static T GetRandomElement<T>(this IList<T> list) {
        return list[Random.Range(0, list.Count)];
    }

    public static T GetRandomElement<T>(this T[] array) {
        return array[Random.Range(0, array.Length)];
    }

    public static T Last<T>(this Collection<T> list) {
        return list[list.Count - 1];
    }
}
}