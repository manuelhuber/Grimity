using System;

namespace Grimity.Data {
public interface IObservable<out T> {
    T Value { get; }
    Action OnChange(Action<T> obs, bool callImmediately = true);
    bool RemoveOnChange(Action<T> obs);
}
}