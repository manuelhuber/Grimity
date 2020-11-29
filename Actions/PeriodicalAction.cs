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
            if (initialDelay) NextExecution = GetTime() + interval;
            _isRunning = value;
        }
    }

    private bool _isRunning;

    /// <summary>
    ///     Return true if the action was successful and the cooldown should be set
    /// </summary>
    public Func<bool> Action;

    public Func<float> GetTime = () => Time.time;


    private void Update() {
        if (!IsRunning) return;
        if (!(GetTime() >= NextExecution)) return;
        if (Action != null && Action.Invoke()) NextExecution = GetTime() + interval;
    }

    public void SetNextExecution(float nextExecution) {
        NextExecution = nextExecution;
    }
}
}