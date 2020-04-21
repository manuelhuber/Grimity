using System.Collections.Generic;

namespace Grimity.ScriptableObject {
public abstract class RuntimeSet<T> : UnityEngine.ScriptableObject {
    public List<T> Items = new List<T>();

    public void Add(T thing) {
        if (!Items.Contains(thing))
            Items.Add(thing);
    }

    public void Remove(T thing) {
        if (Items.Contains(thing))
            Items.Remove(thing);
    }
}
}