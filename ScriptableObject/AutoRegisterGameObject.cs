using UnityEngine;

namespace Grimity.ScriptableObject {
public class AutoRegisterGameObject : MonoBehaviour {
    [SerializeField] public RuntimeGameObjectSet RuntimeSet;

    private void OnEnable() {
        RuntimeSet.Add(gameObject);
    }

    private void OnDisable() {
        RuntimeSet.Remove(gameObject);
    }
}
}