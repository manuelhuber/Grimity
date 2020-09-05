using System.Collections.Generic;
using UnityEngine;

namespace Grimity.ScriptableObject {
[CreateAssetMenu(menuName = "Grimity/runtime set/game object")]
public class RuntimeGameObjectSet : UnityEngine.ScriptableObject {
    [SerializeField] public readonly List<GameObject> Items = new List<GameObject>();

    public void Add(GameObject thing) {
        if (!Items.Contains(thing)) {
            Items.Add(thing);
        }
    }

    public void Remove(GameObject thing) {
        if (Items.Contains(thing)) {
            Items.Remove(thing);
        }
    }
}
}