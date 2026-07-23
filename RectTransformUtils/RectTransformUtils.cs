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

    /// <summary>
    /// Returns the overflow of the target RectTransform relative to the container RectTransform.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector2 GetWorldSpaceOverflow(RectTransform container, RectTransform target) {
        var (containerMin, containerMax) = container.GetMinMaxWorldSpace();
        var (tooltipMin, tooltipMax) = target.GetMinMaxWorldSpace();

        var overflowX = 0f;
        if (tooltipMin.x < containerMin.x) overflowX = containerMin.x - tooltipMin.x;
        else if (tooltipMax.x > containerMax.x) overflowX = containerMax.x - tooltipMax.x;

        var overflowY = 0f;
        if (tooltipMin.y < containerMin.y) overflowY = containerMin.y - tooltipMin.y;
        else if (tooltipMax.y > containerMax.y) overflowY = containerMax.y - tooltipMax.y;

        return new Vector2(overflowX, overflowY);
    }
}
}