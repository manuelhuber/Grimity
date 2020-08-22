using System;
using Ludiq.PeekCore;
using UnityEngine;

namespace Grimity.Actions {
public class DelayedAction : MonoBehaviour {
    private Action _action;
    private Func<float> _getTime = () => Time.time;
    public float TargetTime { get; private set; } = -1;

    public DelayedAction withTime(Func<float> timeFunc) {
        _getTime = timeFunc;
        return this;
    }

    public void After(float delay) {
        TargetTime = _getTime() + delay;
    }

    public void At(float timestamp) {
        TargetTime = timestamp;
    }

    private void Update() {
        if (TargetTime < 0 || !(_getTime() >= TargetTime)) return;
        _action.Invoke();
        Destroy(this);
    }

    public DelayedAction Do(Action action) {
        _action = action;
        return this;
    }
}

public static class DelayedActionExtension {
    public static DelayedAction Do(this MonoBehaviour mono, Action action) {
        return mono.AddComponent<DelayedAction>().Do(action);
    }
}
}