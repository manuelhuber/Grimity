using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grimity.MonoBehaviours {
public abstract class BetterBehaviour : MonoBehaviour {
    private readonly List<Action> _cleanup = new();

    protected void RegisterCleanup(Action action) => _cleanup.Add(action);

    private void OnDestroy() {
        foreach (var action in _cleanup) action();
        OnDestroyed();
    }

    protected virtual void OnDestroyed() {
    }
}
}