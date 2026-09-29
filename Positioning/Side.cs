using System;

namespace Grimity.Positioning {
/// <summary>The side of the reference rect a placed rect sits on.</summary>
public enum Side {
    Top,
    Bottom,
    Left,
    Right
}

public static class SideExtensions {
    public static Side Opposite(this Side side) {
        return side switch {
            Side.Top => Side.Bottom,
            Side.Bottom => Side.Top,
            Side.Left => Side.Right,
            Side.Right => Side.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };
    }

    /// <summary>True for Top/Bottom, where the placed rect is stacked vertically against the reference.</summary>
    public static bool IsVertical(this Side side) => side is Side.Top or Side.Bottom;
}
}
