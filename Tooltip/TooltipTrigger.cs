using UnityEngine;
using UnityEngine.EventSystems;

namespace Grimity.Tooltip {
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private VerticalAlignment VerticalAlignment;
    [SerializeField] private HorizontalAlignment HorizontalAlignment;
    private TooltipData _data;
    private bool _isPointerOver;

    private TooltipManager _tooltipManager;

    private TooltipManager Manager {
        get {
            if (!_tooltipManager) _tooltipManager = TooltipManager.Instance;
            return _tooltipManager;
        }
    }

    private void OnDisable() {
        if (_isPointerOver) Manager.HideTooltip();
    }

    private void OnDestroy() {
        _data?.Dispose();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _isPointerOver = true;
        UpdateTooltip();
    }

    public void OnPointerExit(PointerEventData eventData) {
        Manager.HideTooltip();
        _isPointerOver = false;
    }

    public void SetData(TooltipData data) {
        _data?.Dispose();
        _data = data;
        UpdateTooltip();
    }

    private void UpdateTooltip() {
        if (!_isPointerOver) return;
        if (_data != null) {
            Manager.ShowTooltip(_data, HorizontalAlignment, VerticalAlignment);
        }
    }
}
}