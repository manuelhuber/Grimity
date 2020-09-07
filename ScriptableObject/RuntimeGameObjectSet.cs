using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Grimity.ScriptableObject {
[CreateAssetMenu(menuName = "Grimity/runtime set/game object")]
public class RuntimeGameObjectSet : UnityEngine.ScriptableObject {
    public delegate void OnChangeHandler(ReadOnlyCollection<GameObject> items);

    private readonly List<GameObject> items = new List<GameObject>();
    public ReadOnlyCollection<GameObject> Items => new ReadOnlyCollection<GameObject>(items);

    public event OnChangeHandler OnChange;

    public void Add(GameObject thing) {
        if (items.Contains(thing)) return;
        items.Add(thing);
        PropagateChange();
    }

    public void Remove(GameObject thing) {
        if (!items.Contains(thing)) return;
        items.Remove(thing);
        PropagateChange();
    }

    private void PropagateChange() {
        OnChange?.Invoke(Items);
    }
}
}