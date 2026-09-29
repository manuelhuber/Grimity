using Grimity.Positioning;
using UnityEngine;

namespace Grimity.Tooltip {
/// <summary>
/// Shared position for the tooltips of all <see cref="TooltipTrigger"/>s below it that use
/// <see cref="TooltipReference.ParentAnchor"/>: their tooltips are placed next to this rect instead of the trigger,
/// so e.g. every slot in a row shows its tooltip in the same spot.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class TooltipAnchor : MonoBehaviour {
    [SerializeField] private PlacementConfig placement = PlacementConfig.Below;

    public PlacementConfig Placement {
        get => placement;
        set => placement = value;
    }

    public RectTransform RectTransform => (RectTransform)transform;
}
}
