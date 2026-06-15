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


    public static bool FullyInside(RectTransform viewport, RectTransform target) {
        var viewportRect = viewport.GetWorldRect();
        var (min, max) = target.GetMinMaxWorldSpace();
        return viewportRect.Contains(min) && viewportRect.Contains(max);
    }

    public static bool PartiallyInside(RectTransform viewport, RectTransform target) {
        var viewportRect = viewport.GetWorldRect();
        var childRect = target.GetWorldRect();
        return viewportRect.Overlaps(childRect);
    }

    public static Rect GetWorldRect(this RectTransform rt) {
        return rt.GetWorldRect(new Vector3[4]);
    }

    public static Rect GetWorldRect(this RectTransform rt, Vector3[] corners) {
        rt.GetWorldCorners(corners);
        // corners: [0]=bottom-left, [1]=top-left, [2]=top-right, [3]=bottom-right
        return new Rect(corners[0].x,
            corners[0].y,
            corners[2].x - corners[0].x,
            corners[2].y - corners[0].y);
    }
}
}