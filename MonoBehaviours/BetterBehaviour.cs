using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grimity.MonoBehaviours {
public abstract class BetterBehaviour : MonoBehaviour {
    private readonly List<Action> _cleanup = new();
    private bool _destroyed;

    protected virtual void OnDestroy() {
        Destroy();
    }

    public void RegisterCleanup(Action action) {
        WarnIfCleanupRegisteredBeforeAwake();
        _cleanup.Add(action);
    }

    public void RegisterCleanup(IDisposable disposable) {
        WarnIfCleanupRegisteredBeforeAwake();
        _cleanup.Add(disposable.Dispose);
    }

    private void WarnIfCleanupRegisteredBeforeAwake() {
        if (!didAwake) Debug.LogWarning($"{name}: cleanup registered before Awake! CLEANUP NOT GUARANTEED", this);
    }

    private void Destroy() {
        if (_destroyed) return;
        _destroyed = true;
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

    public void DestroyGameObject() {
        Destroy(gameObject);
        Destroy();
    }
}
}