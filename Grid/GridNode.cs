using System.Collections.Generic;

namespace Grimity.Grid {
/// <summary>
/// </summary>
public struct GridNode {
    public int Index;
    public int X;
    public int Z;

    private Dictionary<string, object> _components;

    private Dictionary<string, object> Components => _components ??= new Dictionary<string, object>();

    public void AddComponent<T>(T component) {
        Components[nameof(T)] = component;
    }

    public T GetComponent<T>() {
        return (T)Components[nameof(T)];
    }
}
}