using System;
using System.Collections.Generic;

namespace Grimity.Data {
/// <summary>
/// Holds no subscription of its own: each observer is wrapped and subscribed directly to the source, so a
/// MappedObservable never outlives its observers and needs no disposing.
/// </summary>
public class MappedObservable<TSource, T> : IObservable<T> {
    private readonly Func<TSource, T> _map;
    private readonly IObservable<TSource> _source;
    private readonly Dictionary<Action<T>, Action<TSource>> _sourceObservers = new();

    public MappedObservable(IObservable<TSource> source, Func<TSource, T> map) {
        _source = source;
        _map = map;
    }

    #region IObservable<T> Members

    public T Value => _map(_source.Value);

    public Action OnChange(Action<T> obs, bool callImmediately = true) {
        if (!_sourceObservers.ContainsKey(obs)) {
            var lastMappedValue = Value;
            Action<TSource> sourceObserver = sourceValue => {
                var mappedValue = _map(sourceValue);
                if (EqualityComparer<T>.Default.Equals(mappedValue, lastMappedValue)) return;
                lastMappedValue = mappedValue;
                obs.Invoke(mappedValue);
            };
            _sourceObservers[obs] = sourceObserver;
            _source.OnChange(sourceObserver, callImmediately: false);
        }

        if (callImmediately) obs.Invoke(Value);

        return () => { RemoveOnChange(obs); };
    }

    public bool RemoveOnChange(Action<T> obs) {
        if (!_sourceObservers.Remove(obs, out var sourceObserver)) return false;
        return _source.RemoveOnChange(sourceObserver);
    }

    #endregion
}
}