using System.Collections.Generic;

namespace Grimity.Collections {
internal class BiDictionary<TFirst, TSecond> {
    private readonly IDictionary<TFirst, TSecond> _firstToSecond = new Dictionary<TFirst, TSecond>();
    private readonly IDictionary<TSecond, TFirst> _secondToFirst = new Dictionary<TSecond, TFirst>();

    // Note potential ambiguity using indexers (e.g. mapping from int to int)
    // Hence the methods as well...
    public TSecond this[TFirst first] => GetByFirst(first);

    public TFirst this[TSecond second] => GetBySecond(second);

    public void Add(TFirst first, TSecond second) {
        if (_firstToSecond.ContainsKey(first)) {
            var currentSecond = _firstToSecond[first];
            _secondToFirst.Remove(currentSecond);
        }

        if (_secondToFirst.ContainsKey(second)) {
            var currentFirst = _secondToFirst[second];
            _firstToSecond.Remove(currentFirst);
        }

        _firstToSecond[first] = second;
        _secondToFirst[second] = first;
    }

    public TSecond GetByFirst(TFirst first) {
        _firstToSecond.TryGetValue(first, out var second);
        return second;
    }

    public TFirst GetBySecond(TSecond second) {
        _secondToFirst.TryGetValue(second, out var first);
        return first;
    }
}
}