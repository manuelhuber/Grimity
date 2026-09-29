using Grimity.Positioning;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Grimity.Tooltip {
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private TooltipReference reference = TooltipReference.Mouse;

    [SerializeField, ShowIf(nameof(reference), TooltipReference.Rect)]
    private RectTransform referenceRect;

    [SerializeField, LabelText("$" + nameof(PlacementLabel))]
    private PlacementConfig placement = PlacementConfig.Cursor;

    [Tooltip("Use the first TooltipAnchor in the parents instead of the reference and placement above, if there is one")]
    [SerializeField] private bool useParentAnchor;

    private TooltipData _data;
    protected bool _isPointerOver;

    private TooltipManager _tooltipManager;

    protected TooltipManager Manager {
        get {
            if (!_tooltipManager) _tooltipManager = TooltipManager.Instance;
            return _tooltipManager;
        }
    }

    private string PlacementLabel => useParentAnchor ? "Placement (without anchor)" : "Placement";

    public TooltipReference Reference {
        get => reference;
        set {
            reference = value;
            UpdateTooltip();
        }
    }

    /// <summary>Rect to place the tooltip next to; setting it switches to <see cref="TooltipReference.Rect"/>.</summary>
    public RectTransform ReferenceRect {
        get => referenceRect;
        set {
            referenceRect = value;
            reference = TooltipReference.Rect;
            UpdateTooltip();
        }
    }

    /// <summary>Placement next to the reference; ignored when a parent <see cref="TooltipAnchor"/> is used.</summary>
    public PlacementConfig Placement {
        get => placement;
        set {
            placement = value;
            UpdateTooltip();
        }
    }

    /// <summary>Whether a <see cref="TooltipAnchor"/> in the parents overrides the reference and placement.</summary>
    public bool UseParentAnchor {
        get => useParentAnchor;
        set {
            useParentAnchor = value;
            UpdateTooltip();
        }
    }

    private void OnDisable() {
        if (_isPointerOver) Manager.HideTooltip();
    }

    protected virtual void OnDestroy() {
        _data?.Dispose();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        var (config, rect) = ResolvePlacement();
        Positioning.Editor.PlacementGizmos.Draw(rect, config);
    }
#endif

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
            var (config, rect) = ResolvePlacement();
            Manager.ShowTooltip(_data, config, rect);
        }
    }

    /// <summary>The placement and reference rect to use; a null rect means the mouse.</summary>
    public (PlacementConfig config, RectTransform reference) ResolvePlacement() {
        if (useParentAnchor) {
            var anchor = GetComponentInParent<TooltipAnchor>();
            if (anchor) return (anchor.Placement, anchor.RectTransform);
        }

        return reference switch {
            TooltipReference.Mouse => (placement, null),
            TooltipReference.Rect when referenceRect => (placement, referenceRect),
            _ => (placement, (RectTransform)transform)
        };
    }
}
}
