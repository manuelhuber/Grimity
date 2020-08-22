using System;
using UnityEngine;

namespace Grimity.Actions {
public class PeriodicalAction : MonoBehaviour {
    /// <summary>
    /// Interval time in Seconds
    /// </summary>
    public float interval;

    public Func<bool> action;
    public bool initialDelay;

    public Func<float> getTime = () => Time.time;

    private bool _isRunning;
    public float NextExecution { get; private set; }

    public bool IsRunning {
        get => _isRunning;
        set {
            if (initialDelay) NextExecution = getTime() + interval;
            _isRunning = value;
        }
    }

    public void SetNextExecution(float nextExecution) {
        NextExecution = nextExecution;
    }

    private void Update() {
        if (!IsRunning) return;
        if (!(getTime() >= NextExecution)) return;
        if (action != null && action.Invoke()) NextExecution = getTime() + interval;
    }
}
}