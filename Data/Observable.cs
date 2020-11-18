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
        // copy to new array since an observer might remove themselves and we aren't allowed to modify
        // a collection during enumeration 
        foreach (var observer in _observers.ToArray()) {
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