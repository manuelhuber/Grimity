using UnityEngine;

namespace Grimity.GameObjects {
public static class TransformExtensions {
    public static void SetY(this Transform transform, float y) {
        var position = transform.position;
        position.y = y;
        transform.position = position;
    }
}
}