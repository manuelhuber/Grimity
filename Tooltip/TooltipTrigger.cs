using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grimity.Tooltip {
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private TooltipManager _tooltipManager;
    private TooltipData _data;
    private Func<TooltipData> _dataProvider;
    public VerticalAlignment VerticalAlignment;
    public HorizontalAlignment HorizontalAlignment;

    public void SetData(TooltipData data) => _data = data;
    public void SetData(Func<TooltipData> dataProvider) => _dataProvider = dataProvider;

    private void Start() {
        _tooltipManager = TooltipManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        var tooltipData = _data ?? _dataProvider?.Invoke();
        if (tooltipData != null) {
            _tooltipManager.ShowTooltip(tooltipData, HorizontalAlignment, VerticalAlignment);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        _tooltipManager.HideTooltip();
    }
}
}