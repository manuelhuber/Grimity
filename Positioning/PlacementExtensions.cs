using UnityEngine;

namespace Grimity.Positioning {
public static class PlacementExtensions {
    /// <summary>
    /// Places a direct child of <paramref name="container"/> next to <paramref name="reference"/>, which must be in
    /// the container's local space. The child's pivot is moved to the edge touching the reference.
    /// </summary>
    public static PlacementResult PlaceNextTo(this RectTransform child,
        Rect reference,
        RectTransform container,
        PlacementConfig config,
        Placement? current = null) {
        var result = PlacementSolver.Solve(reference, child.rect.size, container.rect, config, current);
        child.pivot = result.Pivot;
        child.localPosition = result.PivotPosition;
        return result;
    }
}
}
