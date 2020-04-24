using System;
using UnityEngine;

namespace Grimity.Actions {
public class IntervaledAction : MonoBehaviour {
    public Func<bool> action;
    public float interval;
    public bool initialDelay;

    private float _nextExecution;
    private bool _isRunning;

    public bool IsRunning {
        get => _isRunning;
        set {
            if (initialDelay) {
                _nextExecution = Time.time + interval;
            }

            _isRunning = value;
        }
    }


    private void Update() {
        if (!IsRunning) return;
        if (!(Time.time >= _nextExecution)) return;
        if (action.Invoke()) {
            _nextExecution = Time.time + interval;
        }
    }
}
}