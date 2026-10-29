using Grimity.Data;
using Grimity.RectTransformUtils;
using UnityEngine;

namespace Grimity.Positioning {
/// <summary>
/// Keeps this rect placed next to a reference rect inside its parent. Re-solved every frame, so it adapts when its
/// size changes after spawning (e.g. once a list is filled) and follows the reference when that moves.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class AnchoredPlacement : MonoBehaviour {
    private readonly Observable<Placement?> _chosenPlacement = new(null);
    private PlacementConfig _config;
    private RectTransform _reference;

    /// <summary>
    ///     The candidate of the config the rect is placed at, which differs from the preferred one when that doesn't fit.
    ///     Null until the first placement.
    /// </summary>
    public IObservable<Placement?> ChosenPlacement => _chosenPlacement;

    private void LateUpdate() {
        UpdatePlacement();
    }

    public void Init(RectTransform reference, PlacementConfig config) {
        _reference = reference;
        _config = config;
        _chosenPlacement.Set((Placement?)null);
        UpdatePlacement();
    }

    public void UpdatePlacement() {
        // Keeps its last position once the reference is gone
        if (!_reference || _config == null || transform.parent is not RectTransform container) return;
        var reference = _reference.GetRectIn(container);
        var result = ((RectTransform)transform).PlaceNextTo(reference, container, _config, _chosenPlacement.Value);
        _chosenPlacement.Set(result.Placement);
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