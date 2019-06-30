using UnityEngine;

namespace Grimity.GameObjects {
public class Geometry {
    public static Vector3 LowerCenter(GameObject gameObject) {
        var bounds = CalculateBounds(gameObject);
        return bounds.center + new Vector3(0, -bounds.extents.y, 0);
    }

    public static Bounds CalculateBounds(GameObject gameObject) {
        var bounds = new Bounds();
        bounds.size = Vector3.zero; // reset
        var colliders = gameObject.GetComponentsInChildren<Collider2D>();
        foreach (var col in colliders) {
            bounds.Encapsulate(col.bounds);
        }

        return bounds;
    }
}
}