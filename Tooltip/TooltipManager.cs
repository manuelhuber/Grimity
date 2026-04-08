using System;
using System.Collections.Generic;
using System.Linq;
using Grimity.GameObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Grimity.Tooltip {
public class TooltipManager : MonoBehaviour {
    public static TooltipManager Instance;
    [SerializeField] private List<TooltipView> prefabRegistry;
    [SerializeField] private GameObject TooltipPrefab;
    [SerializeField] private GameObject TooltipContainer;
    [SerializeField] private int margins = 5;


    private GameObject _tooltipUi;
    private RectTransform _uiRectTransform;
    private Dictionary<Type, TooltipView> _prefabMap;
    private RectTransform _mouseTracker;
    private RectTransform _tooltipContainerRectTransform;

    private void Awake() {
        Instance = this;
        _tooltipContainerRectTransform = TooltipContainer.GetComponent<RectTransform>();
        SetupMouseTracker();
        _prefabMap = prefabRegistry.ToDictionary(v => v.DataType, v => v);
        _tooltipUi = Instantiate(TooltipPrefab, _mouseTracker);
        _uiRectTransform = _tooltipUi.gameObject.GetComponent<RectTransform>();
        HideTooltip();
    }

    private void SetupMouseTracker() {
        var mouseTracker = new GameObject("MouseTracker");
        _mouseTracker = mouseTracker.AddComponent<RectTransform>();
        _mouseTracker.SetParent(_tooltipContainerRectTransform, false);
        _mouseTracker.sizeDelta = Vector2.zero;
    }

    private void Update() {
        var mousePos = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _tooltipContainerRectTransform,
            mousePos,
            null,
            out var localPoint
        );
        _mouseTracker.anchoredPosition = localPoint;
    }

    public void ShowTooltip(TooltipData data,
        HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment) {
        SetAnchor(horizontalAlignment, verticalAlignment);
        var type = data.GetType();
        TooltipView prefab = null;
        while (type != null && !_prefabMap.TryGetValue(type, out prefab))
            type = type.BaseType; // fallback up the hierarchy

        _tooltipUi.transform.ClearChildren();

        if (!prefab) {
            Debug.LogError($"No tooltip prefab found for type {type}");
            return;
        }

        var activeTooltip = Instantiate(prefab, _tooltipUi.transform);
        activeTooltip.Bind(data);
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
            HorizontalAlignment.Left => -margins,
            HorizontalAlignment.Middle => 0,
            HorizontalAlignment.Right => margins,
        };
        var posY = verticalAlignment switch {
            VerticalAlignment.Bottom => -margins,
            VerticalAlignment.Middle => 0,
            VerticalAlignment.Top => margins,
        };
        _uiRectTransform.anchorMax = new Vector2(anchorX, anchorY);
        _uiRectTransform.anchorMin = new Vector2(anchorX, anchorY);
        _uiRectTransform.pivot = new Vector2(pivotX, pivotY);
        _uiRectTransform.anchoredPosition = new Vector2(posX, posY);
    }
}
}