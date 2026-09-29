using Grimity.Data;
using Grimity.Positioning;

namespace Grimity.Tooltip {
/// <summary>Converts the old Horizontal/VerticalAlignment pair into a <see cref="PlacementConfig"/>.</summary>
public static class LegacyAlignment {
    public static PlacementConfig ToPlacementConfig(HorizontalAlignment horizontal,
        VerticalAlignment vertical,
        Sides margins = default) {
        // The old alignment named both the edge of the target and the direction the tooltip extends to,
        // e.g. Top/Left = above the target, extending to the left (right edges lined up).
        var placement = vertical switch {
            VerticalAlignment.Top => new Placement(Side.Top, ToAlign(horizontal)),
            VerticalAlignment.Bottom => new Placement(Side.Bottom, ToAlign(horizontal)),
            _ => horizontal switch {
                HorizontalAlignment.Left => new Placement(Side.Left),
                HorizontalAlignment.Right => new Placement(Side.Right),
                // Centered on the target has no equivalent; below is the closest thing that doesn't cover it
                _ => new Placement(Side.Bottom)
            }
        };
        var offset = placement.Side switch {
            Side.Top => margins.Top,
            Side.Bottom => margins.Bottom,
            Side.Left => margins.Left,
            _ => margins.Right
        };
        return new PlacementConfig(placement, FallbackMode.Flip, offset);
    }

    private static Align ToAlign(HorizontalAlignment horizontal) {
        return horizontal switch {
            HorizontalAlignment.Left => Align.End,
            HorizontalAlignment.Right => Align.Start,
            _ => Align.Center
        };
    }
}
}
