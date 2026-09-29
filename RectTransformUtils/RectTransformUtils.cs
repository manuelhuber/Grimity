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

    public static void NudgeInsideParent(this RectTransform rectTransform) {
        rectTransform.NudgeInside(rectTransform.parent as RectTransform);
    }

    public static void NudgeInside(this RectTransform rectTransform, RectTransform container) {
        var overflow = GetWorldSpaceOverflow(container, rectTransform);
        var localOverflow = rectTransform.InverseTransformVector(overflow);
        rectTransform.anchoredPosition += new Vector2(localOverflow.x, localOverflow.y);
    }

    /// <summary>Camera to use for screen conversions of UI under this transform; null for overlay canvases.</summary>
    public static Camera GetCanvasCamera(this Transform transform) {
        var canvas = transform.GetComponentInParent<Canvas>();
        if (!canvas) return null;
        canvas = canvas.rootCanvas;
        return canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    /// <summary>
    /// The bounds of this rect in the local space of <paramref name="space"/> (relative to its pivot, like
    /// <see cref="RectTransform.rect"/>). Goes through screen space, so both may live on different canvases.
    /// </summary>
    public static Rect GetRectIn(this RectTransform rectTransform, RectTransform space) {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        var sourceCamera = rectTransform.GetCanvasCamera();
        var spaceCamera = space.GetCanvasCamera();
        var min = new Vector2(float.MaxValue, float.MaxValue);
        var max = new Vector2(float.MinValue, float.MinValue);
        foreach (var corner in corners) {
            var screenPoint = RectTransformUtility.WorldToScreenPoint(sourceCamera, corner);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(space, screenPoint, spaceCamera, out var local);
            min = Vector2.Min(min, local);
            max = Vector2.Max(max, local);
        }

        return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

    /// <summary>Screen point in the local space of this rect (relative to its pivot).</summary>
    public static Vector2 ScreenToLocal(this RectTransform rectTransform, Vector2 screenPoint) {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            screenPoint,
            rectTransform.GetCanvasCamera(),
            out var local
        );
        return local;
    }
}
}