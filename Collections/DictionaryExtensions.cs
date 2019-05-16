using System;
using System.Collections.Generic;

namespace Grimity.Collections {
public static class DictionaryExtensions {
    public static V GetOrCompute<K, V>(this Dictionary<K, V> dict, K key, Func<K, V> compute) {
        if (!dict.TryGetValue(key, out var value)) {
            value = compute(key);
            dict.Add(key, value);
        }
        return value;
    }
}
}