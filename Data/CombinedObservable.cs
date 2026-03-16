using System;
using System.Collections.Generic;

namespace Grimity.Data {
public class CombinedObservable<T> : Observable<T>, IDisposable {
    private readonly List<Action> _unsubscribers = new List<Action>();

    public CombinedObservable(T value) : base(value) {
    }

    internal void AddUnsubscriber(Action unsubscribe) {
        _unsubscribers.Add(unsubscribe);
    }

    public void Dispose() {
        foreach (var unsubscribe in _unsubscribers) {
            unsubscribe();
        }

        _unsubscribers.Clear();
    }

    public static CombinedObservable<T> From<TA, TB>(
        IObservable<TA> a,
        IObservable<TB> b,
        Func<TA, TB, T> combiner) {
        var result = new CombinedObservable<T>(combiner(a.Value, b.Value));

        Action<TA> updateA = _ => result.Set(combiner(a.Value, b.Value));
        Action<TB> updateB = _ => result.Set(combiner(a.Value, b.Value));

        a.OnChange(updateA, callImmediately: false);
        b.OnChange(updateB, callImmediately: false);

        result.AddUnsubscriber(() => a.RemoveOnChange(updateA));
        result.AddUnsubscriber(() => b.RemoveOnChange(updateB));

        return result;
    }
}
}