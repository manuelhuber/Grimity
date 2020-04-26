using Ludiq.Peek;
using UnityEngine;

namespace Grimity.GameObjects {
public class Geometry {
    public static Vector3 LowerCenter(GameObject gameObject) {
        var bounds = CalculateBounds(gameObject);
        return new Vector3(bounds.center.x, bounds.center.y - bounds.extents.y, bounds.center.z);
    }

    public static Bounds CalculateBounds(GameObject gameObject) {
        var bounds = new Bounds();
        gameObject.CalculateBounds(out bounds, Space.World);
        return bounds;
        bounds.center = gameObject.transform.position;
        bounds.size = Vector3.zero; // reset
        var colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (var col in colliders) bounds.Encapsulate(col.bounds);

        return bounds;
    }
}
}