#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Grimity.Positioning.Editor {
/// <summary>
/// Scene view preview of a <see cref="PlacementConfig"/>: the reference, an example rect at the preferred placement
/// (solid), at each fallback (dotted) and where it would end up right now inside the root canvas (filled).
/// </summary>
public static class PlacementGizmos {
    /// <summary>Size of the example rect, in root canvas units.</summary>
    public static Vector2 PreviewSize = new(120, 50);

    private static readonly Color ReferenceColor = new(1f, 0.8f, 0.2f, 1f);
    private static readonly Color PreferredColor = new(0.25f, 0.6f, 1f, 1f);
    private static readonly Color FallbackColor = new(0.25f, 0.6f, 1f, 0.6f);
    private static readonly Color ResultFill = new(0.25f, 0.6f, 1f, 0.15f);

    public static void Draw(RectTransform reference, PlacementConfig config) {
        if (!reference || config == null) return;
        var canvas = reference.GetComponentInParent<Canvas>();
        if (!canvas) return;
        var space = (RectTransform)canvas.rootCanvas.transform;
        var referenceRect = GetRectIn(reference, space);
        var candidates = config.Candidates().Distinct().ToList();

        Handles.color = FallbackColor;
        foreach (var fallback in candidates.Skip(1)) {
            var corners = Corners(space, PlacementSolver.Place(referenceRect, PreviewSize, fallback, config.Offset));
            Handles.DrawDottedLines(new[] {
                corners[0], corners[1], corners[1], corners[2], corners[2], corners[3], corners[3], corners[0]
            }, 4f);
        }

        var preferred = PlacementSolver.Place(referenceRect, PreviewSize, config.Preferred, config.Offset);
        Handles.DrawSolidRectangleWithOutline(Corners(space, preferred), Color.clear, PreferredColor);

        var result = PlacementSolver.Solve(referenceRect, PreviewSize, space.rect, config);
        Handles.DrawSolidRectangleWithOutline(Corners(space, result.Rect), ResultFill, Color.clear);

        Handles.DrawSolidRectangleWithOutline(Corners(space, referenceRect), Color.clear, ReferenceColor);
    }

    private static Rect GetRectIn(RectTransform rectTransform, RectTransform space) {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        var min = (Vector2)space.InverseTransformPoint(corners[0]);
        var max = (Vector2)space.InverseTransformPoint(corners[2]);
        return Rect.MinMaxRect(Mathf.Min(min.x, max.x), Mathf.Min(min.y, max.y),
            Mathf.Max(min.x, max.x), Mathf.Max(min.y, max.y));
    }

    private static Vector3[] Corners(RectTransform space, Rect rect) {
        return new[] {
            space.TransformPoint(new Vector3(rect.xMin, rect.yMin)),
            space.TransformPoint(new Vector3(rect.xMin, rect.yMax)),
            space.TransformPoint(new Vector3(rect.xMax, rect.yMax)),
            space.TransformPoint(new Vector3(rect.xMax, rect.yMin))
        };
    }
}
}
#endif
