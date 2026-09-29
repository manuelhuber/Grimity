using Grimity.Positioning;
using UnityEngine;

namespace Grimity.Tooltip {
/// <summary>
/// Shared position for the tooltips of all <see cref="TooltipTrigger"/>s below it that have
/// <see cref="TooltipTrigger.UseParentAnchor"/> set: their tooltips are placed next to this rect with this placement,
/// so e.g. every slot in a row shows its tooltip in the same spot. The closest anchor in the parents wins.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class TooltipAnchor : MonoBehaviour {
    [SerializeField] private PlacementConfig placement = PlacementConfig.Below;

    public PlacementConfig Placement {
        get => placement;
        set => placement = value;
    }

    public RectTransform RectTransform => (RectTransform)transform;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Positioning.Editor.PlacementGizmos.Draw(RectTransform, placement);
    }
#endif
}
}
