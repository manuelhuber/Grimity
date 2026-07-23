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
}
}