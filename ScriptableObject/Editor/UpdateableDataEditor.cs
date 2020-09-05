using UnityEditor;
using UnityEngine;

namespace Grimity.ScriptableObject.Editor {
[CustomEditor(typeof(UpdatableData), true)]
public class EndlessTerrainGeneratorInspector : UnityEditor.Editor {
    public override void OnInspectorGUI() {
        var generator = (UpdatableData) target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Clear")) generator.Clear();
    }
}
}