using System;
using UnityEngine;

namespace Grimity.RectTransformUtils {
public static class RectTransformUtils {
    public static (Vector2 min, Vector2 max) GetMinMaxWorldSpace(this RectTransform transform) {
        var corners = new Vector3[4];
        transform.GetWorldCorners(corners);
        var containerMin = new Vector2(corners[0].x, corners[0].y);
        var containerMax = new Vector2(corners[2].x, corners[2].y);
        return new ValueTuple<Vector2, Vector2>(containerMin, containerMax);
    }
}
}