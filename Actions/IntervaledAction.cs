using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grimity.Actions {
public class IntervaledAction : MonoBehaviour {
    public Action action;
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
        _nextExecution = Time.time + interval;
        action.Invoke();
    }
}
}