#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Grimity.Positioning.Editor {
/// <summary>Picks a placement by clicking one of the 12 spots around a box that stands for the reference.</summary>
public class PlacementDrawer : OdinValueDrawer<Placement> {
    private const float Cell = 12f;
    private const float Gap = 2f;
    private static readonly Color Reference = new(0.5f, 0.5f, 0.5f, 0.35f);
    private static readonly Color Unselected = new(0.5f, 0.5f, 0.5f, 0.6f);
    private static readonly Color Hover = new(0.8f, 0.8f, 0.8f, 0.9f);
    private static readonly Color Selected = new(0.25f, 0.6f, 1f, 1f);

    protected override void DrawPropertyLayout(GUIContent label) {
        const float size = 5 * Cell + 4 * Gap;
        var rect = EditorGUILayout.GetControlRect(label != null, size);
        if (label != null) rect = EditorGUI.PrefixLabel(rect, label);
        var origin = rect.position;
        var value = ValueEntry.SmartValue;

        EditorGUI.DrawRect(CellRect(origin, 1, 1, 3, 3), Reference);
        foreach (Side side in System.Enum.GetValues(typeof(Side))) {
            foreach (Align align in System.Enum.GetValues(typeof(Align))) {
                var placement = new Placement(side, align);
                var cell = CellRect(origin, placement);
                var hovered = cell.Contains(Event.current.mousePosition);
                EditorGUI.DrawRect(cell, placement == value ? Selected : hovered ? Hover : Unselected);
                if (hovered && Event.current.type == EventType.MouseDown && Event.current.button == 0) {
                    ValueEntry.SmartValue = placement;
                    GUI.changed = true;
                    Event.current.Use();
                }
            }
        }

        var textRect = new Rect(origin.x + size + 8, rect.y, rect.width - size - 8, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(textRect, $"{value.Side} / {value.Align}", EditorStyles.miniLabel);
        if (Event.current.type == EventType.MouseMove) GUIHelper.RequestRepaint();
    }

    /// <summary>Grid cell of a placement in a 5x5 grid whose middle 3x3 is the reference.</summary>
    private static Rect CellRect(Vector2 origin, Placement placement) {
        var along = (int)placement.Align + 1;
        return placement.Side switch {
            Side.Top => CellRect(origin, along, 0),
            Side.Bottom => CellRect(origin, along, 4),
            Side.Left => CellRect(origin, 0, along),
            _ => CellRect(origin, 4, along)
        };
    }

    private static Rect CellRect(Vector2 origin, int column, int row, int columns = 1, int rows = 1) {
        return new Rect(
            origin.x + column * (Cell + Gap),
            origin.y + row * (Cell + Gap),
            columns * Cell + (columns - 1) * Gap,
            rows * Cell + (rows - 1) * Gap
        );
    }
}
}
#endif
