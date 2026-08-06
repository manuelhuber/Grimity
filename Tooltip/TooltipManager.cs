using System;
using System.Collections.Generic;
using System.Linq;
using Grimity.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using static Grimity.RectTransformUtils.RectTransformUtils;

namespace Grimity.Tooltip {
public class TooltipManager : MonoBehaviour {
    public static TooltipManager Instance;

    [SerializeField] private List<TooltipView> prefabRegistry;
    [SerializeField] public GameObject TooltipContainer;
    [SerializeField] private Sides mouseMargins;
    private TooltipView _activeTooltip;
    private HorizontalAlignment _horizontalAlignment;
    private Sides _margins;

    private Dictionary<Type, TooltipView> _prefabMap;
    private RectTransform _tooltipAnchor;

    private RectTransform _tooltipContainer;
    private GameObject _trackTarget;
    private VerticalAlignment _verticalAlignment;
    private bool IsTrackingMouse => !_trackTarget;

    private void Awake() {
        Instance = this;
        _tooltipContainer = TooltipContainer.GetComponent<RectTransform>();
        SetupAnchor();
        _prefabMap = prefabRegistry.ToDictionary(v => v.DataType, v => v);
    }

    private void Update() {
        if (!_activeTooltip || !_activeTooltip.isActiveAndEnabled) return;
        UpdateAnchorPosition();
    }

    private void SetupAnchor() {
        var mouseTracker = new GameObject("TooltipAnchor");
        _tooltipAnchor = mouseTracker.AddComponent<RectTransform>();
        _tooltipAnchor.SetParent(_tooltipContainer, false);
        _tooltipAnchor.sizeDelta = Vector2.zero;
    }

    private void UpdateAnchorPosition() {
        var screenPos = IsTrackingMouse ? Mouse.current.position.ReadValue() : GetScreenPos(_trackTarget);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _tooltipContainer,
            screenPos,
            null,
            out var localPoint
        );
        _tooltipAnchor.anchoredPosition = localPoint;
        AdjustForBounds(_horizontalAlignment, _verticalAlignment);
    }

    private Vector2 GetScreenPos(GameObject target) {
        var rectTransform = target.transform as RectTransform;
        if (!rectTransform) {
            Debug.LogError("Tracking non RectTransform not yet implemented");
            return Vector2.zero;
        }

        var (min, max) = rectTransform.GetMinMaxWorldSpace();
        var y = _verticalAlignment switch {
            VerticalAlignment.Top => max.y,
            VerticalAlignment.Middle => (max.y + min.y) / 2,
            VerticalAlignment.Bottom => min.y,
            _ => throw new ArgumentOutOfRangeException()
        };
        var x = _horizontalAlignment switch {
            HorizontalAlignment.Left => min.x,
            HorizontalAlignment.Middle => (max.x + min.x) / 2,
            HorizontalAlignment.Right => max.x,
            _ => throw new ArgumentOutOfRangeException()
        };
        return new Vector2(x, y);
    }

    public void ShowTooltip(TooltipData data,
        HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment,
        GameObject trackTarget = null,
        Sides margins = default) {
        _margins = margins;
        _trackTarget = trackTarget;
        _horizontalAlignment = horizontalAlignment;
        _verticalAlignment = verticalAlignment;
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
            Debug.LogError($"No tooltip prefab found for type {type}");
            return;
        }

        _activeTooltip = Instantiate(prefab, _tooltipAnchor);
        _activeTooltip.Bind(data);
        SetAnchor(_horizontalAlignment, _verticalAlignment);
        UpdateAnchorPosition();
        Canvas.ForceUpdateCanvases(); // flush layout so rect sizes are accurate
        AdjustForBounds(_horizontalAlignment, _verticalAlignment);
    }

    public void HideTooltip() {
        _activeTooltip?.gameObject.SetActive(false);
    }

    private void SetAnchor(HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment) {
        var margins = IsTrackingMouse ? mouseMargins : _margins;
        var anchorX = horizontalAlignment.GetAnchor();
        var anchorY = verticalAlignment.GetAnchor();
        var pivotX = horizontalAlignment.GetPivot();
        var pivotY = verticalAlignment.GetPivot();
        var posX = horizontalAlignment switch {
            HorizontalAlignment.Left => -margins.Left,
            HorizontalAlignment.Middle => 0,
            HorizontalAlignment.Right => margins.Right,
        };
        var posY = verticalAlignment switch {
            VerticalAlignment.Bottom => -margins.Bottom,
            VerticalAlignment.Middle => 0,
            VerticalAlignment.Top => margins.Top,
        };
        var rectTransform = _activeTooltip.transform as RectTransform;
        rectTransform.anchorMax = new Vector2(anchorX, anchorY);
        rectTransform.anchorMin = new Vector2(anchorX, anchorY);
        rectTransform.pivot = new Vector2(pivotX, pivotY);
        rectTransform.anchoredPosition = new Vector2(posX, posY);
    }

    private void AdjustForBounds(HorizontalAlignment hAlign, VerticalAlignment vAlign) {
        if (_trackTarget) {
            (_activeTooltip.transform as RectTransform).NudgeInside(_tooltipContainer);
            return;
        }

        SetAnchor(hAlign, vAlign);

        var activeTooltipTransform = _activeTooltip.transform as RectTransform;
        var overflow = GetWorldSpaceOverflow(_tooltipContainer, activeTooltipTransform);

        if (overflow is { x: 0f, y: 0f }) return;

        var (tooltipMin, tooltipMax) = activeTooltipTransform.GetMinMaxWorldSpace();

        var mousePos = new Vector2(_tooltipAnchor.position.x, _tooltipAnchor.position.y);
        var correctedMin = new Vector2(tooltipMin.x + overflow.x, tooltipMin.y + overflow.y);
        var correctedMax = new Vector2(tooltipMax.x + overflow.x, tooltipMax.y + overflow.y);

        var newHAlign = hAlign;
        var newVAlign = vAlign;

        // Per-axis: if the clamped position would put the mouse inside the tooltip, flip instead
        if (overflow.x != 0f && hAlign != HorizontalAlignment.Middle) {
            var wouldOverlapMouse = mousePos.x >= correctedMin.x && mousePos.x <= correctedMax.x;
            if (wouldOverlapMouse) newHAlign = hAlign.Flip();
        }

        if (overflow.y != 0f && vAlign != VerticalAlignment.Middle) {
            var wouldOverlapMouse = mousePos.y >= correctedMin.y && mousePos.y <= correctedMax.y;
            if (wouldOverlapMouse) newVAlign = vAlign.Flip();
        }

        var horizontalFlip = newHAlign != hAlign;
        var verticalFlip = newVAlign != vAlign;

        if (horizontalFlip || verticalFlip) {
            SetAnchor(newHAlign, newVAlign);
        }

        var xNudgeAmount = horizontalFlip ? 0 : overflow.x;
        var yNudgeAmount = verticalFlip ? 0 : overflow.y;

        var worldCorrection = new Vector3(xNudgeAmount, yNudgeAmount, 0f);
        var localCorrection = _tooltipAnchor.InverseTransformVector(worldCorrection);
        activeTooltipTransform.anchoredPosition += new Vector2(localCorrection.x, localCorrection.y);
    }
}
}