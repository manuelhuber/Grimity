using System;
using UnityEngine;

namespace Grimity.Actions {
public class PeriodicalAction : MonoBehaviour {
    /// <summary>
    ///     Interval time in Seconds
    /// </summary>
    public float interval;

    public bool initialDelay;
    public float NextExecution { get; private set; }

    public bool IsRunning {
        get => _isRunning;
        set {
            if (initialDelay) NextExecution = getTime() + interval;
            _isRunning = value;
        }
    }

    private bool _isRunning;

    public Func<bool> action;

    public Func<float> getTime = () => Time.time;

    private void Update() {
        if (!IsRunning) return;
        if (!(getTime() >= NextExecution)) return;
        if (action != null && action.Invoke()) NextExecution = getTime() + interval;
    }

    public void SetNextExecution(float nextExecution) {
        NextExecution = nextExecution;
    }
}
}