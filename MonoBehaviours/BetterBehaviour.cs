using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grimity.MonoBehaviours {
public abstract class BetterBehaviour : MonoBehaviour {
    private readonly List<Action> _cleanup = new();
    protected void RegisterCleanup(Action action) => _cleanup.Add(action);

    private void OnDestroy() {
        Cleanup();
        OnDestroyed();
    }

    protected void Cleanup() {
        foreach (var action in _cleanup) action();
        _cleanup.Clear();
    }

    protected virtual void OnDestroyed() {
    }
}
}