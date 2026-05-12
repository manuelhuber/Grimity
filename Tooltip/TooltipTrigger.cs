using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grimity.Tooltip {
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private VerticalAlignment VerticalAlignment;
    [SerializeField] private HorizontalAlignment HorizontalAlignment;

    private TooltipManager _tooltipManager;
    private TooltipData _data;
    private Func<TooltipData> _dataProvider;
    private bool _isPointerOver;

    private TooltipManager Manager {
        get {
            if (!_tooltipManager) _tooltipManager = TooltipManager.Instance;
            return _tooltipManager;
        }
    }

    public void SetData(TooltipData data) {
        _data = data;
        UpdateTooltip();
    }

    public void SetData(Func<TooltipData> dataProvider) {
        _dataProvider = dataProvider;
        UpdateTooltip();
    }

    private void UpdateTooltip() {
        if (!_isPointerOver) return;
        var tooltipData = _data ?? _dataProvider?.Invoke();
        if (tooltipData != null) {
            Manager.ShowTooltip(tooltipData, HorizontalAlignment, VerticalAlignment);
        }
    }

    private void OnDisable() {
        if (_isPointerOver) Manager.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _isPointerOver = true;
        UpdateTooltip();
    }

    public void OnPointerExit(PointerEventData eventData) {
        Manager.HideTooltip();
        _isPointerOver = false;
    }
}
}