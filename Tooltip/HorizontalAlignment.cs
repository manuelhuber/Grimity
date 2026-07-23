using System;

namespace Grimity.Tooltip {
public enum HorizontalAlignment {
    Left,
    Middle,
    Right
}

public static class HorizontalAlignmentExtensions {
    public static HorizontalAlignment Flip(this HorizontalAlignment alignment) {
        return alignment == HorizontalAlignment.Left ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    }

    public static float GetPivot(this HorizontalAlignment alignment) {
        return alignment switch {
            HorizontalAlignment.Left => 1f,
            HorizontalAlignment.Middle => 0.5f,
            HorizontalAlignment.Right => 0f,
            _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
        };
    }

    public static float GetAnchor(this HorizontalAlignment alignment) {
        return alignment switch {
            HorizontalAlignment.Left => 0f,
            HorizontalAlignment.Middle => 0.5f,
            HorizontalAlignment.Right => 1f,
            _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
        };
    }
}
}