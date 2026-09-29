using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grimity.Positioning {
public readonly struct PlacementResult {
    /// <summary>The placed rect, in the same space as the reference and bounds.</summary>
    public readonly Rect Rect;

    /// <summary>The placement that was chosen from the candidates.</summary>
    public readonly Placement Placement;

    public PlacementResult(Rect rect, Placement placement) {
        Rect = rect;
        Placement = placement;
    }

    /// <summary>Normalized pivot touching the reference; see <see cref="Positioning.Placement.Pivot"/>.</summary>
    public Vector2 Pivot => Placement.Pivot;

    /// <summary>Position of <see cref="Pivot"/> inside <see cref="Rect"/>.</summary>
    public Vector2 PivotPosition => Rect.position + Vector2.Scale(Pivot, Rect.size);
}

/// <summary>
/// Places a rect of a given size next to a reference rect inside some bounds.
/// All rects must be in the same coordinate space (y up).
/// </summary>
/// <remarks>
/// Overflow along the side's axis (e.g. vertical overflow for a rect below the reference) is resolved by trying the
/// next candidate placement; overflow across it is resolved by sliding along the reference's edge. Sliding along the
/// edge can never cover the reference. Only when no candidate fits is the rect clamped into the bounds, overlapping
/// the reference if it has to.
/// </remarks>
public static class PlacementSolver {
    private const float Epsilon = 0.001f;

    /// <param name="current">
    /// The placement currently in use, if any. It is kept as long as it still fits, so a shown rect doesn't jump
    /// between candidates when it or the reference changes slightly.
    /// </param>
    public static PlacementResult Solve(Rect reference,
        Vector2 size,
        Rect bounds,
        PlacementConfig config,
        Placement? current = null) {
        var candidates = config.Candidates().Distinct().ToList();
        if (config.Fallback == FallbackMode.Auto) {
            candidates = candidates
                .OrderByDescending(c => AvailableSpace(reference, bounds, c.Side, config.Offset))
                .ToList();
        }

        if (current.HasValue && candidates.Remove(current.Value)) candidates.Insert(0, current.Value);

        var chosen = Choose(candidates, reference, size, bounds, config.Offset);
        var rect = Place(reference, size, chosen, config.Offset);
        rect = SlideAlongEdge(rect, bounds, chosen.Side);
        rect = ClampInside(rect, bounds);
        return new PlacementResult(rect, chosen);
    }

    /// <summary>The rect at the given placement, ignoring bounds.</summary>
    public static Rect Place(Rect reference, Vector2 size, Placement placement, float offset) {
        float Along(float min, float max, float length) {
            return placement.Align switch {
                Align.Start => min,
                Align.Center => (min + max - length) / 2f,
                _ => max - length
            };
        }

        return placement.Side switch {
            Side.Top => new Rect(Along(reference.xMin, reference.xMax, size.x), reference.yMax + offset, size.x,
                size.y),
            Side.Bottom => new Rect(Along(reference.xMin, reference.xMax, size.x),
                reference.yMin - offset - size.y, size.x, size.y),
            // Start is the top edge, so align from the top down
            Side.Left => new Rect(reference.xMin - offset - size.x,
                reference.yMax - size.y - Along(0, reference.height, size.y), size.x, size.y),
            _ => new Rect(reference.xMax + offset, reference.yMax - size.y - Along(0, reference.height, size.y),
                size.x, size.y)
        };
    }

    /// <summary>Space between the reference's side (plus offset) and the bounds' edge.</summary>
    public static float AvailableSpace(Rect reference, Rect bounds, Side side, float offset) {
        return side switch {
            Side.Top => bounds.yMax - reference.yMax - offset,
            Side.Bottom => reference.yMin - offset - bounds.yMin,
            Side.Left => reference.xMin - offset - bounds.xMin,
            _ => bounds.xMax - reference.xMax - offset
        };
    }

    private static Placement Choose(List<Placement> candidates,
        Rect reference,
        Vector2 size,
        Rect bounds,
        float offset) {
        var best = candidates[0];
        var bestOverflow = float.MaxValue;
        foreach (var candidate in candidates) {
            var length = candidate.Side.IsVertical() ? size.y : size.x;
            var overflow = Mathf.Max(0f, length - AvailableSpace(reference, bounds, candidate.Side, offset));
            if (overflow <= Epsilon) return candidate;
            if (overflow < bestOverflow) {
                best = candidate;
                bestOverflow = overflow;
            }
        }

        return best;
    }

    private static Rect SlideAlongEdge(Rect rect, Rect bounds, Side side) {
        if (side.IsVertical()) rect.x = ClampAxis(rect.x, rect.width, bounds.xMin, bounds.xMax);
        else rect.y = ClampAxisKeepTop(rect.y, rect.height, bounds.yMin, bounds.yMax);
        return rect;
    }

    private static Rect ClampInside(Rect rect, Rect bounds) {
        rect.x = ClampAxis(rect.x, rect.width, bounds.xMin, bounds.xMax);
        rect.y = ClampAxisKeepTop(rect.y, rect.height, bounds.yMin, bounds.yMax);
        return rect;
    }

    /// <summary>Moves [min, min + length] inside [boundsMin, boundsMax]; too long ranges keep their start visible.</summary>
    private static float ClampAxis(float min, float length, float boundsMin, float boundsMax) {
        if (min + length > boundsMax) min = boundsMax - length;
        if (min < boundsMin) min = boundsMin;
        return min;
    }

    /// <summary>Like <see cref="ClampAxis"/>, but too long ranges keep their top (max) visible.</summary>
    private static float ClampAxisKeepTop(float min, float length, float boundsMin, float boundsMax) {
        if (min < boundsMin) min = boundsMin;
        if (min + length > boundsMax) min = boundsMax - length;
        return min;
    }
}
}
