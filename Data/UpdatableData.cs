using System;
using UnityEditor;
using UnityEngine;

namespace Grimity.Data {
public class UpdatableData : ScriptableObject {
    public event Action OnValuesUpdated;
    public bool autoUpdate;

#if UNITY_EDITOR

    protected virtual void OnValidate() {
        if (autoUpdate) {
            EditorApplication.update += NotifyOfUpdatedValues;
        }
    }

    public void NotifyOfUpdatedValues() {
        EditorApplication.update -= NotifyOfUpdatedValues;
        if (OnValuesUpdated != null) {
            OnValuesUpdated();
        }
    }

#endif
}
}