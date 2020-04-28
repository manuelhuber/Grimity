using System;
using System.Collections.Generic;

namespace Grimity.Data {
public class Observable<T> : IObservable<T> {
    private readonly List<Action<T>> _observers = new List<Action<T>>();

    public T Value { get; private set; }

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

    public bool OnChange(Action<T> obs, bool callImmediately = true) {
        if (_observers.Contains(obs)) return false;
        _observers.Add(obs);
        if (callImmediately) obs.Invoke(Value);
        return true;
    }

    public bool RemoveOnChange(Action<T> obs) {
        return _observers.Remove(obs);
    }
}
}