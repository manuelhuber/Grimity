using System;
using UnityEngine;

namespace Grimity.Positioning {
/// <summary>
/// Where a rect is placed relative to a reference rect: the side it sits on, and which point of its edge meets which
/// point of the reference's edge. E.g. Top with <see cref="ReferenceAlign"/> Center and <see cref="Align"/> End puts
/// the rect above the reference with its right end at the reference's center.
/// </summary>
[Serializable]
public struct Placement : IEquatable<Placement> {
    public Side Side;

    /// <summary>The point on the reference's edge that the placed rect is aligned to.</summary>
    public Align ReferenceAlign;

    /// <summary>The point on the placed rect's edge that sits on <see cref="ReferenceAlign"/>.</summary>
    public Align Align;

    /// <summary>Lines up the same points of both edges, e.g. both centers or both start ends.</summary>
    public Placement(Side side, Align align = Align.Center) : this(side, align, align) {
    }

    public Placement(Side side, Align referenceAlign, Align align) {
        Side = side;
        ReferenceAlign = referenceAlign;
        Align = align;
    }

    public Placement Flipped => new(Side.Opposite(), ReferenceAlign, Align);

    /// <summary>
    /// Normalized point of the placed rect that touches the reference, e.g. (0.5, 1) for Bottom/Center.
    /// Scaling around this pivot grows the rect away from the reference.
    /// </summary>
    public Vector2 Pivot {
        get {
            var along = Align.NormalizedPosition();
            return Side switch {
                Side.Top => new Vector2(along, 0f),
                Side.Bottom => new Vector2(along, 1f),
                // Start is the top edge on vertical edges
                Side.Left => new Vector2(1f, 1f - along),
                Side.Right => new Vector2(0f, 1f - along),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public bool Equals(Placement other) =>
        Side == other.Side && ReferenceAlign == other.ReferenceAlign && Align == other.Align;

    public override bool Equals(object obj) => obj is Placement other && Equals(other);
    public override int GetHashCode() => ((int)Side * 397 ^ (int)ReferenceAlign) * 397 ^ (int)Align;
    public static bool operator ==(Placement a, Placement b) => a.Equals(b);
    public static bool operator !=(Placement a, Placement b) => !a.Equals(b);
    public override string ToString() => $"{Side}/{ReferenceAlign}->{Align}";
}
}