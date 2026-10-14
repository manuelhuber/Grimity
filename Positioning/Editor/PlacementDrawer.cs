#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Grimity.Positioning.Editor {
/// <summary>
/// Picks a placement with two grids. The left box stands for the reference: click one of the 12 spots around it to
/// pick the side and the point on the reference's edge. The right box stands for the placed rect: click one of the 3
/// spots on its edge facing the reference to pick which of its points sits there.
/// </summary>
public class PlacementDrawer : OdinValueDrawer<Placement> {
    private const float Cell = 12f;
    private const float Gap = 2f;
    private const float GridSize = 5 * Cell + 4 * Gap;
    private const float GridSpacing = 16f;
    private static readonly Color Box = new(0.5f, 0.5f, 0.5f, 0.35f);
    private static readonly Color Unselected = new(0.5f, 0.5f, 0.5f, 0.6f);
    private static readonly Color Hover = new(0.8f, 0.8f, 0.8f, 0.9f);
    private static readonly Color Selected = new(0.25f, 0.6f, 1f, 1f);

    protected override void DrawPropertyLayout(GUIContent label) {
        var rect = EditorGUILayout.GetControlRect(label != null, GridSize);
        if (label != null) rect = EditorGUI.PrefixLabel(rect, label);
        var referenceGrid = rect.position;
        var placedGrid = referenceGrid + new Vector2(GridSize + GridSpacing, 0);
        var value = ValueEntry.SmartValue;

        EditorGUI.DrawRect(CellRect(referenceGrid, 1, 1, 3, 3), Box);
        foreach (Side side in System.Enum.GetValues(typeof(Side))) {
            foreach (Align referenceAlign in System.Enum.GetValues(typeof(Align))) {
                var cell = CellRect(referenceGrid, side, referenceAlign);
                var isSelected = side == value.Side && referenceAlign == value.ReferenceAlign;
                if (DrawSpot(cell, isSelected)) SetValue(new Placement(side, referenceAlign, value.Align));
            }
        }

        EditorGUI.DrawRect(CellRect(placedGrid, 1, 1, 3, 3), Box);
        // The placed rect's edge facing the reference is on the reference's opposite side
        var facingEdge = value.Side.Opposite();
        foreach (Align align in System.Enum.GetValues(typeof(Align))) {
            var cell = CellRect(placedGrid, facingEdge, align);
            if (DrawSpot(cell, align == value.Align)) SetValue(new Placement(value.Side, value.ReferenceAlign, align));
        }

        var textX = placedGrid.x + GridSize + 8;
        var textRect = new Rect(textX, rect.y, rect.xMax - textX, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(textRect, value.ToString(), EditorStyles.miniLabel);
        if (Event.current.type == EventType.MouseMove) GUIHelper.RequestRepaint();
    }

    /// <summary>Draws a clickable spot and returns whether it was clicked.</summary>
    private static bool DrawSpot(Rect cell, bool isSelected) {
        var isHovered = cell.Contains(Event.current.mousePosition);
        EditorGUI.DrawRect(cell, isSelected ? Selected : isHovered ? Hover : Unselected);
        if (!isHovered || Event.current.type != EventType.MouseDown || Event.current.button != 0) return false;
        Event.current.Use();
        return true;
    }

    private void SetValue(Placement placement) {
        ValueEntry.SmartValue = placement;
        GUI.changed = true;
    }

    /// <summary>Grid cell of a point on an edge of the box in the middle 3x3 of a 5x5 grid.</summary>
    private static Rect CellRect(Vector2 origin, Side edge, Align align) {
        var along = (int)align + 1;
        return edge switch {
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