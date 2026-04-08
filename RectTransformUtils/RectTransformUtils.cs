using System;
using UnityEngine;

namespace Grimity.RectTransformUtils {
public static class RectTransformUtils {
    public static Tuple<Vector2, Vector2> GetMinMaxWorldSpace(this RectTransform transform) {
        var cCorners = new Vector3[4];
        transform.GetWorldCorners(cCorners);
        var containerMin = new Vector2(cCorners[0].x, cCorners[0].y);
        var containerMax = new Vector2(cCorners[2].x, cCorners[2].y);
        return new(containerMin, containerMax);
    }
}
}