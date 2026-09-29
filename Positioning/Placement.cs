using System;
using UnityEngine;

namespace Grimity.Positioning {
/// <summary>Where a rect is placed relative to a reference rect: the side it sits on and how it aligns along it.</summary>
[Serializable]
public struct Placement : IEquatable<Placement> {
    public Side Side;
    public Align Align;

    public Placement(Side side, Align align = Align.Center) {
        Side = side;
        Align = align;
    }

    public Placement Flipped => new(Side.Opposite(), Align);

    /// <summary>
    /// Normalized point of the placed rect that touches the reference, e.g. (0.5, 1) for Bottom/Center.
    /// Scaling around this pivot grows the rect away from the reference.
    /// </summary>
    public Vector2 Pivot {
        get {
            var along = Align switch {
                Align.Start => 0f,
                Align.Center => 0.5f,
                Align.End => 1f,
                _ => throw new ArgumentOutOfRangeException()
            };
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

    public bool Equals(Placement other) => Side == other.Side && Align == other.Align;
    public override bool Equals(object obj) => obj is Placement other && Equals(other);
    public override int GetHashCode() => ((int)Side * 397) ^ (int)Align;
    public static bool operator ==(Placement a, Placement b) => a.Equals(b);
    public static bool operator !=(Placement a, Placement b) => !a.Equals(b);
    public override string ToString() => $"{Side}/{Align}";
}
}
