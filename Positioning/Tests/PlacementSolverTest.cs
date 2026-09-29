using NUnit.Framework;
using UnityEngine;

namespace Grimity.Positioning.Tests {
public class PlacementSolverTest {
    // 100 x 100 screen, y up
    private static readonly Rect Bounds = new(0, 0, 100, 100);
    private static readonly Vector2 Size = new(20, 10);

    private static Rect RefAt(float x, float y, float w = 10, float h = 10) => new(x, y, w, h);

    [Test]
    public void PreferredFits_IsUsed() {
        var result = PlacementSolver.Solve(RefAt(45, 45), Size, Bounds, PlacementConfig.Below);
        Assert.AreEqual(new Placement(Side.Bottom), result.Placement);
        Assert.AreEqual(new Rect(40, 35, 20, 10), result.Rect);
    }

    [TestCase(Side.Top, Align.Start, 45, 55)]
    [TestCase(Side.Top, Align.Center, 40, 55)]
    [TestCase(Side.Top, Align.End, 35, 55)]
    [TestCase(Side.Bottom, Align.Start, 45, 35)]
    [TestCase(Side.Left, Align.Start, 25, 45)]
    [TestCase(Side.Left, Align.Center, 25, 45)]
    [TestCase(Side.Right, Align.End, 55, 45)]
    public void Place_PositionsAgainstReference(Side side, Align align, float x, float y) {
        var rect = PlacementSolver.Place(RefAt(45, 45), Size, new Placement(side, align), 0);
        Assert.AreEqual(new Rect(x, y, Size.x, Size.y), rect);
    }

    [Test]
    public void Place_VerticalAlignOnTallReference() {
        var reference = RefAt(45, 20, 10, 60);
        Assert.AreEqual(70, PlacementSolver.Place(reference, Size, new Placement(Side.Right, Align.Start), 0).y);
        Assert.AreEqual(45, PlacementSolver.Place(reference, Size, new Placement(Side.Right), 0).y);
        Assert.AreEqual(20, PlacementSolver.Place(reference, Size, new Placement(Side.Right, Align.End), 0).y);
    }

    [Test]
    public void Offset_AddsGap() {
        var result = PlacementSolver.Solve(RefAt(45, 45), Size, Bounds, PlacementConfig.Below.WithOffset(3));
        Assert.AreEqual(32, result.Rect.y);
    }

    [Test]
    public void NoRoomBelow_FlipsAbove() {
        var result = PlacementSolver.Solve(RefAt(45, 5), Size, Bounds, PlacementConfig.Below);
        Assert.AreEqual(new Placement(Side.Top), result.Placement);
        Assert.AreEqual(15, result.Rect.y);
    }

    [Test]
    public void FallbackNone_KeepsPreferredAndClamps() {
        var config = PlacementConfig.Below.WithFallback(FallbackMode.None);
        var result = PlacementSolver.Solve(RefAt(45, 5), Size, Bounds, config);
        Assert.AreEqual(new Placement(Side.Bottom), result.Placement);
        Assert.AreEqual(0, result.Rect.y);
    }

    [Test]
    public void Custom_TriesFallbacksInOrder() {
        // Reference in the bottom left corner: below and left don't fit, right does
        var config = PlacementConfig.Below
            .Or(new Placement(Side.Left))
            .Or(new Placement(Side.Right))
            .Or(new Placement(Side.Top));
        var result = PlacementSolver.Solve(RefAt(2, 2), Size, Bounds, config);
        Assert.AreEqual(new Placement(Side.Right), result.Placement);
    }

    [Test]
    public void Auto_PicksSideWithMostSpace() {
        var config = PlacementConfig.Below.WithFallback(FallbackMode.Auto);
        // Room below (60) fits, but there is more room to the left (80)
        var result = PlacementSolver.Solve(RefAt(80, 60), Size, Bounds, config);
        Assert.AreEqual(new Placement(Side.Left), result.Placement);
    }

    [Test]
    public void CrossAxisOverflow_SlidesWithoutCoveringReference() {
        // Reference at the right edge: centered below would stick out to the right
        var reference = RefAt(92, 50, 8);
        var result = PlacementSolver.Solve(reference, Size, Bounds, PlacementConfig.Below);
        Assert.AreEqual(new Placement(Side.Bottom), result.Placement);
        Assert.AreEqual(new Rect(80, 40, 20, 10), result.Rect);
        Assert.IsFalse(result.Rect.Overlaps(reference));
    }

    [Test]
    public void NothingFits_PicksLeastOverflowAndOverlaps() {
        var tall = new Vector2(20, 60);
        // 40 above, 50 below: neither fits, below overflows less
        var reference = RefAt(45, 50);
        var result = PlacementSolver.Solve(reference, tall, Bounds, PlacementConfig.Above);
        Assert.AreEqual(new Placement(Side.Bottom), result.Placement);
        Assert.AreEqual(0, result.Rect.yMin);
        Assert.IsTrue(result.Rect.Overlaps(reference));
    }

    [Test]
    public void TallerThanBounds_KeepsTopVisible() {
        var result = PlacementSolver.Solve(RefAt(45, 45), new Vector2(20, 150), Bounds, PlacementConfig.Below);
        Assert.AreEqual(100, result.Rect.yMax);
    }

    [Test]
    public void WiderThanBounds_KeepsLeftVisible() {
        var result = PlacementSolver.Solve(RefAt(45, 45), new Vector2(150, 10), Bounds, PlacementConfig.Below);
        Assert.AreEqual(0, result.Rect.xMin);
    }

    [Test]
    public void Current_IsKeptWhileItFits() {
        // Both fit; preferred is below but the tooltip is currently shown above
        var current = new Placement(Side.Top);
        var result = PlacementSolver.Solve(RefAt(45, 45), Size, Bounds, PlacementConfig.Below, current);
        Assert.AreEqual(current, result.Placement);
    }

    [Test]
    public void Current_IsDroppedWhenItNoLongerFits() {
        var current = new Placement(Side.Top);
        var result = PlacementSolver.Solve(RefAt(45, 85), Size, Bounds, PlacementConfig.Below, current);
        Assert.AreEqual(new Placement(Side.Bottom), result.Placement);
    }

    [Test]
    public void Current_NotInCandidates_IsIgnored() {
        var current = new Placement(Side.Left);
        var result = PlacementSolver.Solve(RefAt(45, 45), Size, Bounds, PlacementConfig.Below, current);
        Assert.AreEqual(new Placement(Side.Bottom), result.Placement);
    }

    [Test]
    public void PointReference_CursorPlacementExtendsUpLeft() {
        var result = PlacementSolver.Solve(new Rect(50, 50, 0, 0), Size, Bounds, PlacementConfig.Cursor);
        Assert.AreEqual(new Rect(30, 50, 20, 10), result.Rect);
        Assert.AreEqual(new Vector2(1, 0), result.Pivot);
        Assert.AreEqual(new Vector2(50, 50), result.PivotPosition);
    }

    [Test]
    public void PointReference_NearTop_FlipsBelowCursor() {
        var result = PlacementSolver.Solve(new Rect(50, 95, 0, 0), Size, Bounds, PlacementConfig.Cursor);
        Assert.AreEqual(new Placement(Side.Bottom, Align.End), result.Placement);
        Assert.AreEqual(85, result.Rect.y);
    }

    [Test]
    public void PresetsAreNotShared() {
        var modified = PlacementConfig.Below.OrAbove().WithOffset(5);
        Assert.AreEqual(FallbackMode.Flip, PlacementConfig.Below.Fallback);
        Assert.AreEqual(0, PlacementConfig.Below.Offset);
        Assert.AreEqual(1, modified.Fallbacks.Count);
    }

    [TestCase(Side.Top, Align.Start, 0, 0)]
    [TestCase(Side.Bottom, Align.Center, 0.5f, 1)]
    [TestCase(Side.Left, Align.Start, 1, 1)]
    [TestCase(Side.Right, Align.End, 0, 0)]
    public void Pivot_TouchesReference(Side side, Align align, float x, float y) {
        Assert.AreEqual(new Vector2(x, y), new Placement(side, align).Pivot);
    }
}
}
