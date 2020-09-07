using UnityEngine;

namespace Grimity.ScriptableObject {
public class AutoRegisterGameObject : MonoBehaviour {
    [SerializeField] public RuntimeGameObjectSet runtimeSet;

    private void OnEnable() {
        runtimeSet.Add(gameObject);
    }

    private void OnDisable() {
        runtimeSet.Remove(gameObject);
    }

    private void OnDestroy() {
        runtimeSet.Remove(gameObject);
    }
}
}