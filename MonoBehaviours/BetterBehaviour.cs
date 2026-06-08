using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grimity.MonoBehaviours {
public abstract class BetterBehaviour : MonoBehaviour {
    private readonly List<Action> _cleanup = new();
    protected void RegisterCleanup(Action action) => _cleanup.Add(action);
    protected void RegisterCleanup(IDisposable disposable) => _cleanup.Add(disposable.Dispose);

    private void OnDestroy() {
        Cleanup();
        OnDestroyed();
    }

    protected void Cleanup() {
        foreach (var action in _cleanup) {
            try {
                action();
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
        }

        _cleanup.Clear();
    }

    protected virtual void OnDestroyed() {
    }
}
}