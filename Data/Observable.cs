using System;
using System.Collections.Generic;

namespace Grimity.Data {
public class Observable<T> : IObservable<T> {
    private readonly List<Action<T>> _observers = new List<Action<T>>();

    public Observable(T value) {
        Value = value;
    }

    public void Set(Func<T, T> next) {
        Set(next(Value));
    }

    public void Set(T next) {
        if (IsSameValue(next)) return;
        Value = next;
        // copy to new array since an observer might remove themselves and we aren't allowed to modify
        // a collection during enumeration 
        foreach (var observer in _observers.ToArray()) {
            observer.Invoke(Value);
        }
    }

    private bool IsSameValue(T next) {
        if (next == null) return Value == null;
        return next.Equals(Value);
    }

    public static implicit operator T(Observable<T> optional) {
        return optional.Value;
    }

    #region IObservable<T> Members

    public T Value { get; private set; }

    public Action OnChange(Action<T> obs, bool callImmediately = true) {
        if (!_observers.Contains(obs)) {
            _observers.Add(obs);
        }

        if (callImmediately) obs.Invoke(Value);

        return () => { RemoveOnChange(obs); };
    }

    public bool RemoveOnChange(Action<T> obs) {
        return _observers.Remove(obs);
    }

    #endregion
}
}