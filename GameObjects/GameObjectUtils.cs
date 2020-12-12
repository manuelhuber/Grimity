using UnityEngine;

namespace Grimity.GameObjects {
public static class GameObjectUtils {
    public static T GetComp<T>(Component component) {
        return component.GetComponent<T>();
    }

    public static bool NotNull(Component component) {
        return component != null;
    }

    public static bool NotNull(GameObject component) {
        return component != null;
    }

    public static void ClearChildren(this GameObject gameObject) {
        gameObject.transform.ClearChildren();
    }

    public static void ClearChildren(this Transform trans) {
        foreach (Transform child in trans) {
            Object.Destroy(child.gameObject);
        }
    }
}
}