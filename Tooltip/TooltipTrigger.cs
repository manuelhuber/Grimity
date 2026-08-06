using Grimity.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grimity.Tooltip {
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private VerticalAlignment VerticalAlignment;
    [SerializeField] private HorizontalAlignment HorizontalAlignment;
    [SerializeField] private GameObject target;
    [SerializeField] private Sides targetMargins;

    private TooltipData _data;
    protected bool _isPointerOver;

    private TooltipManager _tooltipManager;

    protected TooltipManager Manager {
        get {
            if (!_tooltipManager) _tooltipManager = TooltipManager.Instance;
            return _tooltipManager;
        }
    }

    private void OnDisable() {
        if (_isPointerOver) Manager.HideTooltip();
    }

    protected virtual void OnDestroy() {
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

    protected virtual void UpdateTooltip() {
        if (!_isPointerOver) return;
        if (_data != null) {
            Manager.ShowTooltip(_data, HorizontalAlignment, VerticalAlignment, target, targetMargins);
        }
    }
}
}