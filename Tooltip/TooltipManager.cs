using System;
using System.Collections.Generic;
using System.Linq;
using Grimity.Data;
using Grimity.Positioning;
using Grimity.RectTransformUtils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Grimity.Tooltip {
public class TooltipManager : MonoBehaviour {
    public static TooltipManager Instance;

    [SerializeField] private List<TooltipView> prefabRegistry;
    [SerializeField] public GameObject TooltipContainer;

    [Tooltip("Area around the mouse hotspot that tooltips following the mouse keep clear of, e.g. the cursor sprite")]
    [SerializeField] private Sides mouseMargins;

    private TooltipView _activeTooltip;
    private PlacementConfig _config;

    /// <summary>Placement last used for a rect reference; kept while it fits so the tooltip doesn't jump.</summary>
    private Placement? _currentPlacement;

    private Dictionary<Type, TooltipView> _prefabMap;
    private RectTransform _reference;
    private RectTransform _tooltipContainer;

    private bool IsTrackingMouse => !_reference;

    private void Awake() {
        Instance = this;
        _tooltipContainer = TooltipContainer.GetComponent<RectTransform>();
        _prefabMap = prefabRegistry.ToDictionary(v => v.DataType, v => v);
    }

    private void LateUpdate() {
        if (!_activeTooltip || !_activeTooltip.isActiveAndEnabled) return;
        UpdatePosition();
    }

    /// <summary>Shows a tooltip next to <paramref name="reference"/>, or next to the mouse if it is null.</summary>
    public void ShowTooltip(TooltipData data, PlacementConfig config, RectTransform reference = null) {
        _config = config;
        _reference = reference;
        _currentPlacement = null;
        var type = data.GetType();
        TooltipView prefab = null;
        while (type != null && !_prefabMap.TryGetValue(type, out prefab))
            type = type.BaseType; // fallback up the hierarchy

        if (_activeTooltip) {
            _activeTooltip.Dispose();
            Destroy(_activeTooltip.gameObject);
            _activeTooltip = null;
        }

        if (!prefab) {
            Debug.LogError($"No tooltip prefab found for type {data.GetType()}");
            return;
        }

        _activeTooltip = Instantiate(prefab, _tooltipContainer);
        var rectTransform = (RectTransform)_activeTooltip.transform;
        rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _activeTooltip.Bind(data);
        Canvas.ForceUpdateCanvases(); // flush layout so rect sizes are accurate
        UpdatePosition();
    }

    /// <summary>Old alignment API; kept until all triggers use <see cref="PlacementConfig"/>.</summary>
    public void ShowTooltip(TooltipData data,
        HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment,
        GameObject trackTarget = null,
        Sides margins = default) {
        var reference = trackTarget ? trackTarget.transform as RectTransform : null;
        var config = LegacyAlignment.ToPlacementConfig(horizontalAlignment,
            verticalAlignment,
            reference ? margins : default);
        ShowTooltip(data, config, reference);
    }

    public void HideTooltip() {
        _activeTooltip?.gameObject.SetActive(false);
    }

    private void UpdatePosition() {
        var tooltip = (RectTransform)_activeTooltip.transform;
        if (IsTrackingMouse) {
            // Solved from the preferred placement every frame, so it flips back once there's room again
            tooltip.PlaceNextTo(GetMouseRect(), _tooltipContainer, _config);
            return;
        }

        var reference = _reference.GetRectIn(_tooltipContainer);
        _currentPlacement = tooltip.PlaceNextTo(reference, _tooltipContainer, _config, _currentPlacement).Placement;
    }

    private Rect GetMouseRect() {
        var mouse = _tooltipContainer.ScreenToLocal(Mouse.current.position.ReadValue());
        return Rect.MinMaxRect(
            mouse.x - mouseMargins.Left,
            mouse.y - mouseMargins.Bottom,
            mouse.x + mouseMargins.Right,
            mouse.y + mouseMargins.Top
        );
    }
}
}
