using UnityEditor;
using UnityEngine;

namespace Grimity.Data.Editor {
[CustomEditor(typeof(UpdatableData), true)]
public class EndlessTerrainGeneratorInspector : UnityEditor.Editor {
    public override void OnInspectorGUI() {
        var generator = (UpdatableData) target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Clear")) {
            generator.Clear();
        }
    }
}
}