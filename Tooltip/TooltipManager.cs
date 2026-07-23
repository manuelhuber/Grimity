using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Grimity.RectTransformUtils.RectTransformUtils;

namespace Grimity.Tooltip {
public class TooltipManager : MonoBehaviour {
    public static TooltipManager Instance;
    [SerializeField] private List<TooltipView> prefabRegistry;
    [SerializeField] private GameObject TooltipPrefab;
    [SerializeField] public GameObject TooltipContainer;
    [SerializeField] private int horizontalMarginLeft = 5;
    [SerializeField] private int horizontalMarginRight = 5;
    [SerializeField] private int verticalMarginTop = 5;
    [SerializeField] private int verticalMarginBottom = 5;
    private TooltipView _activeTooltip;
    private HorizontalAlignment _horizontalAlignment;
    private RectTransform _mouseTracker;
    private Dictionary<Type, TooltipView> _prefabMap;
    private RectTransform _tooltipContainerRectTransform;

    private GameObject _tooltipUi;
    private RectTransform _uiRectTransform;
    private VerticalAlignment _verticalAlignment;

    private void Awake() {
        Instance = this;
        _tooltipContainerRectTransform = TooltipContainer.GetComponent<RectTransform>();
        SetupMouseTracker();
        _prefabMap = prefabRegistry.ToDictionary(v => v.DataType, v => v);
        _tooltipUi = Instantiate(TooltipPrefab, _mouseTracker);
        _uiRectTransform = _tooltipUi.gameObject.GetComponent<RectTransform>();
        HideTooltip();
    }

    private void Update() {
        if (!_tooltipUi.activeSelf) return;
        var mousePos = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _tooltipContainerRectTransform,
            mousePos,
            null,
            out var localPoint
        );
        _mouseTracker.anchoredPosition = localPoint;
        AdjustForBounds(_horizontalAlignment, _verticalAlignment);
    }

    private void SetupMouseTracker() {
        var mouseTracker = new GameObject("MouseTracker");
        _mouseTracker = mouseTracker.AddComponent<RectTransform>();
        _mouseTracker.SetParent(_tooltipContainerRectTransform, false);
        _mouseTracker.sizeDelta = Vector2.zero;
    }

    public void ShowTooltip(TooltipData data,
        HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment) {
        _horizontalAlignment = horizontalAlignment;
        _verticalAlignment = verticalAlignment;
        SetAnchor(_horizontalAlignment, _verticalAlignment);
        Canvas.ForceUpdateCanvases(); // flush layout so rect sizes are accurate
        AdjustForBounds(_horizontalAlignment, _verticalAlignment);
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

        _activeTooltip = Instantiate(prefab, _tooltipUi.transform);
        _activeTooltip.Bind(data);
        ShowTooltip();
    }

    private void ShowTooltip() {
        _tooltipUi.gameObject.SetActive(true);
    }

    public void HideTooltip() {
        _tooltipUi.gameObject.SetActive(false);
    }

    private void SetAnchor(HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment) {
        var anchorX = horizontalAlignment switch {
            HorizontalAlignment.Left => 0f,
            HorizontalAlignment.Middle => 0.5f,
            HorizontalAlignment.Right => 1f,
        };
        var anchorY = verticalAlignment switch {
            VerticalAlignment.Bottom => 0f,
            VerticalAlignment.Middle => 0.5f,
            VerticalAlignment.Top => 1f,
        };
        var pivotX = horizontalAlignment switch {
            HorizontalAlignment.Left => 1f,
            HorizontalAlignment.Middle => 0.5f,
            HorizontalAlignment.Right => 0f,
        };
        var pivotY = verticalAlignment switch {
            VerticalAlignment.Bottom => 1f,
            VerticalAlignment.Middle => 0.5f,
            VerticalAlignment.Top => 0f,
        };
        var posX = horizontalAlignment switch {
            HorizontalAlignment.Left => -horizontalMarginLeft,
            HorizontalAlignment.Middle => 0,
            HorizontalAlignment.Right => horizontalMarginRight,
        };
        var posY = verticalAlignment switch {
            VerticalAlignment.Bottom => -verticalMarginBottom,
            VerticalAlignment.Middle => 0,
            VerticalAlignment.Top => verticalMarginTop,
        };
        _uiRectTransform.anchorMax = new Vector2(anchorX, anchorY);
        _uiRectTransform.anchorMin = new Vector2(anchorX, anchorY);
        _uiRectTransform.pivot = new Vector2(pivotX, pivotY);
        _uiRectTransform.anchoredPosition = new Vector2(posX, posY);
    }

    private void AdjustForBounds(HorizontalAlignment hAlign, VerticalAlignment vAlign) {
        SetAnchor(hAlign, vAlign);

        var overflow = GetWorldSpaceOverflow(_tooltipContainerRectTransform, _uiRectTransform);

        if (overflow is { x: 0f, y: 0f }) return;

        var (tooltipMin, tooltipMax) = _uiRectTransform.GetMinMaxWorldSpace();
        var mousePos = new Vector2(_mouseTracker.position.x, _mouseTracker.position.y);

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
        var localCorrection = _mouseTracker.InverseTransformVector(worldCorrection);
        _uiRectTransform.anchoredPosition += new Vector2(localCorrection.x, localCorrection.y);
    }
}
}