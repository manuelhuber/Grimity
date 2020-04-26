using System;
using UnityEngine;

namespace Grimity.Actions {
public class IntervaledAction : MonoBehaviour {
    private bool _isRunning;

    private float _nextExecution;
    public Func<bool> action;
    public bool initialDelay;
    public float interval;

    public bool IsRunning {
        get => _isRunning;
        set {
            if (initialDelay) _nextExecution = Time.time + interval;

            _isRunning = value;
        }
    }


    private void Update() {
        if (!IsRunning) return;
        if (!(Time.time >= _nextExecution)) return;
        if (action.Invoke()) _nextExecution = Time.time + interval;
    }
}
}