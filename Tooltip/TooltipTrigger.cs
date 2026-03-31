using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grimity.Tooltip {
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private TooltipManager _tooltipManager;
    public Func<TooltipData> dataProvider;

    private void Start() {
        _tooltipManager = TooltipManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _tooltipManager.ShowTooltip(dataProvider.Invoke());
    }

    public void OnPointerExit(PointerEventData eventData) {
        _tooltipManager.HideTooltip();
    }
}
}