using System;

namespace Grimity.Positioning {
/// <summary>
/// A point along an edge. Top/Bottom edges: Start = left end, End = right end.
/// Left/Right edges: Start = top end, End = bottom end.
/// </summary>
public enum Align {
    Start,
    Center,
    End
}

public static class AlignExtensions {
    /// <summary>Position of the point along its edge: 0 at the start, 1 at the end.</summary>
    public static float NormalizedPosition(this Align align) {
        return align switch {
            Align.Start => 0f,
            Align.Center => 0.5f,
            Align.End => 1f,
            _ => throw new ArgumentOutOfRangeException(nameof(align), align, null)
        };
    }
}
}