using NUnit.Framework;
using Unity.Collections;
using Unity.Mathematics;

namespace Grimity.Grid.Tests {
public readonly struct MapParams {
    public MapParams(float cellSize,
                     int worldSizeX,
                     int worldSizeZ,
                     float3 worldPositionCenter) {
        _cellSize = cellSize;
        _cellCountX = worldSizeX;
        _cellCountZ = worldSizeZ;
        _worldPositionCenter = worldPositionCenter;
    }

    private readonly float _cellSize;
    private readonly int _cellCountX;
    private readonly int _cellCountZ;
    private readonly float3 _worldPositionCenter;

    public Grid ToMap() {
        return new Grid(_cellSize, _cellCountX, _cellCountZ, _worldPositionCenter, Allocator.Temp);
    }
}

public class GridMapTest {
    private static readonly MapParams Map4x4 = new(2, 4, 4, float3.zero);

    private static object[] _gridToWorldCases = {
        new object[] { Map4x4, 0, 0, new float3(-3, 0, -3) },
        new object[] { Map4x4, 0, 3, new float3(-3, 0, 3) },
        new object[] { Map4x4, 3, 0, new float3(3, 0, -3) },
        new object[] { Map4x4, 3, 3, new float3(3, 0, 3) }
    };

    private static object[] _worldToGridCases = {
        // Exactly between nodes
        new object[] { Map4x4, new float3(2, 0, 2), 3, 3 },
        new object[] { Map4x4, new float3(-2, 0, 2), 1, 3 },
        new object[] { Map4x4, new float3(2, 0, -2), 3, 1 },
        new object[] { Map4x4, new float3(-2, 0, -2), 1, 1 },

        // Definitely in one cell over another
        new object[] { Map4x4, new float3(1.9f, 0, 2.1f), 2, 3 },
        new object[] { Map4x4, new float3(-1.9f, 0, 2.1f), 1, 3 },
        new object[] { Map4x4, new float3(1.9f, 0, -2.1f), 2, 0 },
        new object[] { Map4x4, new float3(-1.9f, 0, -2.1f), 1, 0 },
        new object[] { Map4x4, new float3(2.1f, 0, 1.9f), 3, 2 },
        new object[] { Map4x4, new float3(-2.1f, 0, 1.9f), 0, 2 },
        new object[] { Map4x4, new float3(2.1f, 0, -1.9f), 3, 1 },
        new object[] { Map4x4, new float3(-2.1f, 0, -1.9f), 0, 1 },

        // Min & Max positions
        new object[] { Map4x4, new float3(4f, 0, 4f), 3, 3 },
        new object[] { Map4x4, new float3(-4f, 0, 4f), 0, 3 },
        new object[] { Map4x4, new float3(4f, 0, -4f), 3, 0 },
        new object[] { Map4x4, new float3(-4f, 0, -4f), 0, 0 },

        // Out of range
        new object[] { Map4x4, new float3(-5f, 0, -5f), -1, -1 },
        new object[] { Map4x4, new float3(5f, 0, 5f), -1, -1 },
        new object[] { Map4x4, new float3(-100f, 0, -100f), -1, -1 },
        new object[] { Map4x4, new float3(100f, 0, 100f), -1, -1 }
    };

    [TestCaseSource(nameof(_gridToWorldCases))]
    public void GridToWorld(MapParams mapParams,
                            int x,
                            int z,
                            float3 pos) {
        var map = mapParams.ToMap();
        Assert.AreEqual(pos, map.GridToWorldPosition(x, z));
        map.Dispose();
    }

    [TestCaseSource(nameof(_gridToWorldCases))]
    public void GridToWorldToGrid(MapParams mapParams,
                                  int x,
                                  int z,
                                  float3 _) {
        var map = mapParams.ToMap();
        var worldPos = map.GridToWorldPosition(x, z);
        Assert.AreEqual(new int2(x, z), map.WorldPositionToGridCoordinates(worldPos));
        map.Dispose();
    }

    [TestCaseSource(nameof(_worldToGridCases))]
    public void WorldToGrid(MapParams mapParams,
                            float3 pos,
                            int x,
                            int z) {
        var map = mapParams.ToMap();
        Assert.AreEqual(new int2(x, z), map.WorldPositionToGridCoordinates(pos));
        map.Dispose();
    }
}
}