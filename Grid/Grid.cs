using System;
using System.Collections.Generic;
using Grimity.Loops;
using Grimity.Math;
using Grimity.NativeCollections;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Grimity.Grid {
public class Grid {
    private readonly float3 _lowerLeftCorner;

    private NativeArray<GridNode> _map;

    // Just a simple flag so that we can avoid accessing the map after disposing
    private bool _mapIsDisposed;

    public Grid(float cellSize,
                int cellCountX,
                int cellCountZ,
                float3 worldPositionCenter,
                Allocator persistence = Allocator.Persistent) {
        CellSize = cellSize;
        WorldSizeX = cellCountX * cellSize;
        WorldSizeZ = cellCountZ * cellSize;
        CellCountX = cellCountX;
        CellCountZ = cellCountZ;
        _map = new NativeArray<GridNode>(CellCountX * CellCountZ, persistence);
        new Loop2D(CellCountX, CellCountZ).LoopY((x, z) => {
            var index = x + CellCountX * z;
            _map[index] = new GridNode { Index = index, X = x, Z = z };
        });
        _lowerLeftCorner = worldPositionCenter - new float3(WorldSizeX / 2, 0, WorldSizeZ / 2);
        Size = new int2(CellCountX, CellCountZ);
    }

    public float CellSize { get; }
    public float WorldSizeX { get; }
    public float WorldSizeZ { get; }
    public int2 Center => new(Mathf.RoundToInt(CellCountX / 2f), Mathf.RoundToInt(CellCountZ / 2f));

    public NativeArray<GridNode> Nodes => _map;
    public int2 Size { get; }
    public int CellCountX { get; }
    public int CellCountZ { get; }

    public void AddComponent<T>(Func<int, int, T> generator) {
        new Loop2D(CellCountX, CellCountZ).LoopY((x, z) => {
            var index = x + CellCountX * z;
            _map[index].AddComponent(generator.Invoke(x, z));
        });
    }

    public float3 GridToWorldPosition(int x, int z) {
        return new float3(x * CellSize - WorldSizeX / 2 + CellSize / 2,
            0,
            z * CellSize - WorldSizeZ / 2 + CellSize / 2);
    }

    public GridNode WorldPositionToGridNode(float3 pos) {
        var coord = WorldPositionToGridCoordinates(pos);
        return GetNode(coord.x, coord.y);
    }

    public int2 WorldPositionToGridCoordinates(float3 pos) {
        var xDistance = pos.x - _lowerLeftCorner.x;
        var zDistance = pos.z - _lowerLeftCorner.z;

        var percentageX = xDistance / WorldSizeX;
        var percentageZ = zDistance / WorldSizeZ;
        if (percentageX is > 1 or < 0 || percentageZ is > 1 or < 0) {
            // throw new ArgumentOutOfRangeException();
            return new int2(-1, -1);
        }

        var x = Mathf.FloorToInt(percentageX * CellCountX);
        var z = Mathf.FloorToInt(percentageZ * CellCountZ);
        return new int2(x.MinMax(0, CellCountX - 1), z.MinMax(0, CellCountZ - 1));
    }

    public GridNode GetNode(int x, int z) {
        return _mapIsDisposed ? new GridNode() : _map.Get2D(z, x, CellCountX);
    }

    public GridNode GetNodeByIndex(int i) {
        return _map[i];
    }

    public float3 IndexToWorldPosition(int i) {
        var node = GetNodeByIndex(i);
        return GridToWorldPosition(node.X, node.Z);
    }

    public void SetNode(GridNode node) {
        _map.Put2D(node, node.Z, node.X, CellCountX);
    }

    public List<GridNode> GetAreaAroundPosition(float3 pos, int2 size) {
        if (size.x <= 1 && size.y <= 1) {
            return new List<GridNode> { WorldPositionToGridNode(pos) };
        }

        if (size.x.IsEven()) {
            pos.x -= CellSize / 2;
        }

        if (size.y.IsEven()) {
            pos.z -= CellSize / 2;
        }

        var worldPositionToNode = WorldPositionToGridCoordinates(pos);
        var startX = Mathf.RoundToInt(size.x.IsEven()
                ? worldPositionToNode.x - (size.x / 2 - 1)
                : worldPositionToNode.x - Mathf.Floor(size.x / 2f))
            ;
        var startY = Mathf.RoundToInt(size.y.IsEven()
                ? worldPositionToNode.y - (size.y / 2 - 1)
                : worldPositionToNode.y - Mathf.Floor(size.y / 2f))
            ;
        var list = new List<GridNode>();

        for (var x = startX; x < startX + size.x; x++) {
            for (var y = startY; y < startY + size.y; y++) {
                list.Add(GetNode(x, y));
            }
        }

        return list;
    }

    public void Dispose() {
        _map.Dispose();
        _mapIsDisposed = true;
    }
}
}