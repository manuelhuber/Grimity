using UnityEngine;

namespace Grimity.GameObjects {
public class GameObjectUtils {
    public static T GetComp<T>(Component component) {
        return component.GetComponent<T>();
    }

    public static bool NotNull(Component component) {
        return component != null;
    }

    public static bool NotNull(GameObject component) {
        return component != null;
    }
}
}