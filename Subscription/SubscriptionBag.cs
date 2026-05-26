using System;
using System.Collections.Generic;

namespace Grimity.Subscription {
public class SubscriptionBag {
    private readonly List<Action> _cleanup = new();
    public void RegisterCleanup(Action action) => _cleanup.Add(action);
    public void RegisterCleanup(IDisposable disposable) => _cleanup.Add(disposable.Dispose);

    public void Cleanup() {
        foreach (var action in _cleanup) action();
        _cleanup.Clear();
    }
}

public static class SubscriptionBagUtils {
    public static void AddTo(this Action action, SubscriptionBag bag) {
        bag.RegisterCleanup(action);
    }

    public static void AddTo(this IDisposable disposable, SubscriptionBag bag) {
        bag.RegisterCleanup(disposable);
    }
}
}