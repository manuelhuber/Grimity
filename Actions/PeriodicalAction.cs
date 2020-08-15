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
    private float _nextExecution;

    public bool IsRunning {
        get => _isRunning;
        set {
            if (initialDelay) _nextExecution = getTime() + interval;
            _isRunning = value;
        }
    }


    private void Update() {
        if (!IsRunning) return;
        if (!(getTime() >= _nextExecution)) return;
        if (action != null && action.Invoke()) _nextExecution = getTime() + interval;
    }
}
}