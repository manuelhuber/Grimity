namespace Grimity.Tooltip {
/// <summary>What a <see cref="TooltipTrigger"/>'s tooltip is placed next to.</summary>
public enum TooltipReference {
    /// <summary>The mouse cursor; the tooltip follows it.</summary>
    Mouse,

    /// <summary>The trigger's own rect.</summary>
    Self,

    /// <summary>Another rect, set on the trigger.</summary>
    Rect,

    /// <summary>The first <see cref="TooltipAnchor"/> in the trigger's parents, using the anchor's placement.</summary>
    ParentAnchor
}
}
