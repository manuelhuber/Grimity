using System.Collections.Generic;

namespace Grimity.Collections {
class BiDictionary<TFirst, TSecond> {
    IDictionary<TFirst, TSecond> firstToSecond = new Dictionary<TFirst, TSecond>();
    IDictionary<TSecond, TFirst> secondToFirst = new Dictionary<TSecond, TFirst>();

    public void Add(TFirst first, TSecond second) {
        if (firstToSecond.ContainsKey(first)) {
            var currentSecond = firstToSecond[first];
            secondToFirst.Remove(currentSecond);
        }

        if (secondToFirst.ContainsKey(second)) {
            var currentFirst = secondToFirst[second];
            firstToSecond.Remove(currentFirst);
        }

        firstToSecond[first] = second;
        secondToFirst[second] = first;
    }

    // Note potential ambiguity using indexers (e.g. mapping from int to int)
    // Hence the methods as well...
    public TSecond this[TFirst first] => GetByFirst(first);

    public TFirst this[TSecond second] => GetBySecond(second);

    public TSecond GetByFirst(TFirst first) {
        firstToSecond.TryGetValue(first, out var second);
        return second;
    }

    public TFirst GetBySecond(TSecond second) {
        secondToFirst.TryGetValue(second, out var first);
        return first;
    }
}
}