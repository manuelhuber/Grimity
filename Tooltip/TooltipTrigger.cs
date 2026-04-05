using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grimity.Tooltip {
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private TooltipManager _tooltipManager;
    private TooltipData _data;
    private Func<TooltipData> _dataProvider;

    public void SetData(TooltipData data) => _data = data;
    public void SetData(Func<TooltipData> dataProvider) => _dataProvider = dataProvider;

    private void Start() {
        _tooltipManager = TooltipManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _tooltipManager.ShowTooltip(_data ?? _dataProvider?.Invoke());
    }

    public void OnPointerExit(PointerEventData eventData) {
        _tooltipManager.HideTooltip();
    }
}
}