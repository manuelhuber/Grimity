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

    public void SetData(TooltipData data) => _data = data;
    public void SetData(Func<TooltipData> dataProvider) => _dataProvider = dataProvider;

    private void Start() {
        _tooltipManager = TooltipManager.Instance;
    }

    private void OnDisable() {
        if (_isPointerOver) _tooltipManager.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _isPointerOver = true;
        var tooltipData = _data ?? _dataProvider?.Invoke();
        if (tooltipData != null) {
            _tooltipManager.ShowTooltip(tooltipData, HorizontalAlignment, VerticalAlignment);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        _tooltipManager.HideTooltip();
        _isPointerOver = false;
    }
}
}