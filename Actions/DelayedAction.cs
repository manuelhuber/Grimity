using System;
using System.Collections;
using UnityEngine;

namespace Grimity.Actions {
public class DelayedAction {
    private float _delay;
    private MonoBehaviour _mono;
    private Action _action;

    public DelayedAction(float delay, MonoBehaviour mono) {
        _delay = delay;
        _mono = mono;
    }

    public void Do(Action action) {
        _action = action;
        _mono.StartCoroutine(DoCor());
    }

    private IEnumerator DoCor() {
        yield return new WaitForSeconds(_delay);
        _action.Invoke();
    }
}

public static class DelayedActionExtension {
    public static DelayedAction After(this MonoBehaviour mono, float seconds) {
        return new DelayedAction(seconds, mono);
    }
}
}