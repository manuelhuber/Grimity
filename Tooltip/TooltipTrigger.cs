using Grimity.Data;
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

    // Old alignment settings, converted into the fields above by MigrateLegacyPlacement
    [SerializeField, HideInInspector] private VerticalAlignment VerticalAlignment;
    [SerializeField, HideInInspector] private HorizontalAlignment HorizontalAlignment;
    [SerializeField, HideInInspector] private GameObject target;
    [SerializeField, HideInInspector] private Sides targetMargins;
    [SerializeField, HideInInspector] private bool legacyPlacementMigrated;

    private TooltipData _data;
    protected bool _isPointerOver;

    private TooltipManager _tooltipManager;

    protected TooltipManager Manager {
        get {
            if (!_tooltipManager) _tooltipManager = TooltipManager.Instance;
            return _tooltipManager;
        }
    }

    private string PlacementLabel =>
        reference == TooltipReference.ParentAnchor ? "Placement (without anchor)" : "Placement";

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

    protected virtual void Awake() {
        MigrateLegacyPlacement();
    }

    private void OnDisable() {
        if (_isPointerOver) Manager.HideTooltip();
    }

    protected virtual void OnDestroy() {
        _data?.Dispose();
    }

    private void OnValidate() {
        MigrateLegacyPlacement();
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
        switch (reference) {
            case TooltipReference.Mouse:
                return (placement, null);
            case TooltipReference.Rect:
                if (referenceRect) return (placement, referenceRect);
                break;
            case TooltipReference.ParentAnchor:
                var anchor = GetComponentInParent<TooltipAnchor>();
                if (anchor) return (anchor.Placement, anchor.RectTransform);
                break;
        }

        return (placement, (RectTransform)transform);
    }

    /// <summary>
    /// Converts the old Vertical/HorizontalAlignment settings, once. Runs on load, so prefabs that haven't been
    /// re-saved since still work; returns whether anything changed.
    /// </summary>
    public bool MigrateLegacyPlacement() {
        if (legacyPlacementMigrated) return false;
        legacyPlacementMigrated = true;
        var targetRect = target ? target.transform as RectTransform : null;
        if (!targetRect) {
            // Mouse tooltips got their gap from the manager, which still applies it around the cursor
            reference = TooltipReference.Mouse;
            placement = LegacyAlignment.ToPlacementConfig(HorizontalAlignment, VerticalAlignment);
        } else {
            reference = targetRect == transform ? TooltipReference.Self : TooltipReference.Rect;
            referenceRect = reference == TooltipReference.Rect ? targetRect : null;
            placement = LegacyAlignment.ToPlacementConfig(HorizontalAlignment, VerticalAlignment, targetMargins);
        }

        target = null;
        return true;
    }
}
}
