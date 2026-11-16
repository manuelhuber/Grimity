using System;
using System.Collections.Generic;
using System.Linq;
using Grimity.Collections;

namespace Grimity.Rng {
public class MarbleBag<T> {
    private readonly List<T> _marbles;
    private int _nextMarbleIndex;

    public MarbleBag(IEnumerable<T> marbles) {
        _marbles = marbles.ToList();
        if (_marbles.Count == 0) {
            throw new ArgumentException("A marble bag needs at least one marble.", nameof(marbles));
        }

        _marbles.ShuffleInPlace();
    }

    public T Next() {
        if (_nextMarbleIndex == _marbles.Count) Refill();

        var marble = _marbles[_nextMarbleIndex];
        _nextMarbleIndex++;
        return marble;
    }

    private void Refill() {
        var lastDrawnMarble = _marbles[^1];
        _marbles.ShuffleInPlace();
        _nextMarbleIndex = 0;
        if (IsSameMarble(_marbles[0], lastDrawnMarble)) SwapAwayFromFirstPosition(lastDrawnMarble);
    }

    // Without this, the last marble of one bag could come out again as the first marble of the next
    private void SwapAwayFromFirstPosition(T lastDrawnMarble) {
        var otherMarbleIndices = Enumerable.Range(1, _marbles.Count - 1)
            .Where(i => !IsSameMarble(_marbles[i], lastDrawnMarble))
            .ToList();
        if (otherMarbleIndices.Count == 0) return;

        var swapIndex = otherMarbleIndices.GetRandomElement();
        (_marbles[0], _marbles[swapIndex]) = (_marbles[swapIndex], _marbles[0]);
    }

    private static bool IsSameMarble(T a, T b) => EqualityComparer<T>.Default.Equals(a, b);
}
}