using System;
using UnityEditor;

namespace Grimity.ScriptableObject {
public class UpdatableData : UnityEngine.ScriptableObject {
    public bool autoUpdate;
    public event Action OnValuesUpdated;

    public void Clear() {
        OnValuesUpdated = null;
    }

#if UNITY_EDITOR

    protected virtual void OnValidate() {
        if (autoUpdate) EditorApplication.update += NotifyOfUpdatedValues;
    }

    public void NotifyOfUpdatedValues() {
        EditorApplication.update -= NotifyOfUpdatedValues;
        if (OnValuesUpdated != null) OnValuesUpdated();
    }

#endif
}
}