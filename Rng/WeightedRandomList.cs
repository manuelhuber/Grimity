using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Grimity.Rng {
public class WeightedRandomList<T> {
    private readonly List<(T item, int weight)> _entries = new();
    private int _totalWeight;

    public int Count => _entries.Count;
    public int TotalWeight => _totalWeight;

    public WeightedRandomList<T> Add(T item, int weight) {
        if (weight <= 0) throw new ArgumentException("Weight must be greater than zero.", nameof(weight));

        _entries.Add((item, weight));
        _totalWeight += weight;
        return this;
    }

    public T Pick() {
        if (_entries.Count == 0)
            throw new InvalidOperationException("WeightedList is empty.");
        var roll = Random.Range(0, _totalWeight);

        foreach (var (item, weight) in _entries) {
            roll -= weight;
            if (roll < 0)
                return item;
        }

        return _entries[^1].item;
    }
}
}