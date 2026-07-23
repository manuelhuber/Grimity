using System;

namespace Grimity.Tooltip {
public enum VerticalAlignment {
    Top,
    Middle,
    Bottom
}

public static class VerticalAlignmentExtensions {
    public static VerticalAlignment Flip(this VerticalAlignment alignment) {
        return alignment == VerticalAlignment.Top ? VerticalAlignment.Bottom : VerticalAlignment.Top;
    }

    public static float GetPivot(this VerticalAlignment alignment) {
        return alignment switch {
            VerticalAlignment.Bottom => 1f,
            VerticalAlignment.Middle => 0.5f,
            VerticalAlignment.Top => 0f,
            _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
        };
    }

    public static float GetAnchor(this VerticalAlignment alignment) {
        return alignment switch {
            VerticalAlignment.Bottom => 0f,
            VerticalAlignment.Middle => 0.5f,
            VerticalAlignment.Top => 1f,
            _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
        };
    }
}
}