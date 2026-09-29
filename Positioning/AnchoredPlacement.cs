using Grimity.RectTransformUtils;
using UnityEngine;

namespace Grimity.Positioning {
/// <summary>
/// Keeps this rect placed next to a reference rect inside its parent. Re-solved every frame, so it adapts when its
/// size changes after spawning (e.g. once a list is filled) and follows the reference when that moves.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class AnchoredPlacement : MonoBehaviour {
    private PlacementConfig _config;
    private Placement? _current;
    private RectTransform _reference;

    private void LateUpdate() {
        UpdatePlacement();
    }

    public void Init(RectTransform reference, PlacementConfig config) {
        _reference = reference;
        _config = config;
        _current = null;
        UpdatePlacement();
    }

    public void UpdatePlacement() {
        // Keeps its last position once the reference is gone
        if (!_reference || _config == null || transform.parent is not RectTransform container) return;
        var reference = _reference.GetRectIn(container);
        _current = ((RectTransform)transform).PlaceNextTo(reference, container, _config, _current).Placement;
    }
}

public static class AnchoredPlacementExtensions {
    /// <summary>Keeps this rect next to <paramref name="reference"/>; see <see cref="AnchoredPlacement"/>.</summary>
    public static AnchoredPlacement KeepNextTo(this RectTransform rectTransform,
        RectTransform reference,
        PlacementConfig config) {
        var placement = rectTransform.GetComponent<AnchoredPlacement>();
        if (!placement) placement = rectTransform.gameObject.AddComponent<AnchoredPlacement>();
        placement.Init(reference, config);
        return placement;
    }
}
}
