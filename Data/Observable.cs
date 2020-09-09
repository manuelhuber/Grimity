using System;
using System.Collections.Generic;

namespace Grimity.Data {
public class Observable<T> : IObservable<T> {
    private readonly List<Action<T>> _observers = new List<Action<T>>();

    public Observable(T value) {
        Value = value;
    }

    public void Set(T next) {
        if (next.Equals(Value)) return;
        Value = next;
        foreach (var observer in _observers) {
            observer.Invoke(Value);
        }
    }

    #region IObservable<T> Members

    public T Value { get; private set; }

    public void OnChange(Action<T> obs, bool callImmediately = true) {
        if (!_observers.Contains(obs)) {
            _observers.Add(obs);
        }

        if (callImmediately) obs.Invoke(Value);
    }

    public bool RemoveOnChange(Action<T> obs) {
        return _observers.Remove(obs);
    }

    #endregion
}
}