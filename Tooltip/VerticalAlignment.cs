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
}
}