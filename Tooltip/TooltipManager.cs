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

    private GameObject _tooltipUi;
    private RectTransform _uiRectTransform;
    private Dictionary<Type, TooltipView> _prefabMap;

    private void Awake() {
        Instance = this;
        _prefabMap = prefabRegistry.ToDictionary(v => v.DataType, v => v);
        _tooltipUi = Instantiate(TooltipPrefab, TooltipContainer.transform);
        _uiRectTransform = _tooltipUi.gameObject.GetComponent<RectTransform>();
        HideTooltip();
    }

    private void Update() {
        var mousePos = Mouse.current.position.ReadValue();
        _uiRectTransform.position = new Vector3(mousePos.x - 10, mousePos.y - 10, 0);
    }

    public void ShowTooltip(TooltipData data) {
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
}
}