using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Grimity.Positioning {
/// <summary>
/// Preferred placement of a rect next to a reference, plus what to try if it doesn't fit.
/// The static presets are shared: use the With/Or methods, which return copies, to derive from them.
/// </summary>
[Serializable]
public class PlacementConfig {
    public Placement Preferred = new(Side.Bottom);
    public FallbackMode Fallback = FallbackMode.Flip;

    [ShowIf(nameof(Fallback), FallbackMode.Custom)]
    public List<Placement> Fallbacks = new();

    /// <summary>Gap between the reference and the placed rect.</summary>
    public float Offset;

    public PlacementConfig() {
    }

    public PlacementConfig(Placement preferred, FallbackMode fallback = FallbackMode.Flip, float offset = 0f) {
        Preferred = preferred;
        Fallback = fallback;
        Offset = offset;
    }

    public static PlacementConfig Below => new(new Placement(Side.Bottom));
    public static PlacementConfig Above => new(new Placement(Side.Top));
    public static PlacementConfig LeftOf => new(new Placement(Side.Left));
    public static PlacementConfig RightOf => new(new Placement(Side.Right));

    /// <summary>Up and to the left of a point reference such as the mouse cursor.</summary>
    public static PlacementConfig Cursor => new(new Placement(Side.Top, Align.End));

    /// <summary>Adds a fallback, switching to <see cref="FallbackMode.Custom"/>.</summary>
    public PlacementConfig Or(Placement placement) {
        var copy = Copy();
        if (copy.Fallback != FallbackMode.Custom) {
            copy.Fallback = FallbackMode.Custom;
            copy.Fallbacks.Clear();
        }

        copy.Fallbacks.Add(placement);
        return copy;
    }

    public PlacementConfig OrAbove() => Or(new Placement(Side.Top, Preferred.Align));
    public PlacementConfig OrBelow() => Or(new Placement(Side.Bottom, Preferred.Align));

    public PlacementConfig WithFallback(FallbackMode fallback) {
        var copy = Copy();
        copy.Fallback = fallback;
        return copy;
    }

    public PlacementConfig WithOffset(float offset) {
        var copy = Copy();
        copy.Offset = offset;
        return copy;
    }

    public PlacementConfig WithAlign(Align align) {
        var copy = Copy();
        copy.Preferred.Align = align;
        return copy;
    }

    public PlacementConfig Copy() {
        return new PlacementConfig(Preferred, Fallback, Offset) {
            Fallbacks = new List<Placement>(Fallbacks ?? new List<Placement>())
        };
    }

    /// <summary>Placements to try, in order. Auto mode is ordered by the solver instead.</summary>
    public IEnumerable<Placement> Candidates() {
        yield return Preferred;
        switch (Fallback) {
            case FallbackMode.Flip:
                yield return Preferred.Flipped;
                break;
            case FallbackMode.Custom when Fallbacks != null:
                foreach (var fallback in Fallbacks) yield return fallback;
                break;
            case FallbackMode.Auto:
                foreach (Side side in System.Enum.GetValues(typeof(Side))) {
                    if (side != Preferred.Side) yield return new Placement(side, Preferred.Align);
                }

                break;
        }
    }
}
}
