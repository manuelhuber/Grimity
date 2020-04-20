using System;
using System.Collections.Generic;

namespace Grimity.Collections {
public static class DictionaryExtensions {
    public static TV GetOrCompute<TK, TV>(this Dictionary<TK, TV> dict, TK key, Func<TK, TV> compute) {
        if (dict.TryGetValue(key, out var value)) return value;
        value = compute(key);
        dict.Add(key, value);
        return value;
    }

    public static TV GetOrDefault<TK, TV>(this Dictionary<TK, TV> dict, TK key, TV fallback) {
        return !dict.TryGetValue(key, out var value) ? fallback : value;
    }
}
}