using System;
using Grimity.Loops;
using UnityEngine;

namespace Grimity.Mesh {
public class MeshGenerator {
    public static MeshData GenerateMesh(float[,] heightMap,
                                        float heightAmplifier,
                                        AnimationCurve heightCurve,
                                        int levelOfDetail = 0,
                                        // Distance between vertices
                                        float scale = 1f) {
        var increment = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);

        var widthVerticesCount = (width - 1) / increment + 1;
        var heightVerticesCount = (height - 1) / increment + 1;

        // VERTICES & UVS ------------
        var vertices = new Vector3[widthVerticesCount * heightVerticesCount];
        var uvs = new Vector2[widthVerticesCount * heightVerticesCount];
        var index = 0;

        new Loop2D(width, height, i => i + increment).loopY((x, y) => {
            var elevation = heightCurve.Evaluate(heightMap[x, y]) * heightAmplifier;
            vertices[index] = new Vector3(x * scale, elevation, y * scale);
            uvs[index] = new Vector2(x / (float) width, y / (float) height);
            index++;
        });

        // TRIANGLES ------------
        var triangles = new int[3 * 2 * (widthVerticesCount - 1) * (heightVerticesCount - 1)];
        index = 0;
        new Loop2D(widthVerticesCount - 1, heightVerticesCount - 1).loopX((x, y) => {
            var topLeft = heightVerticesCount * x + y;
            var topRight = topLeft + 1;
            var bottomRight = topRight + heightVerticesCount;
            var bottomLeft = topLeft + heightVerticesCount;
            triangles[index++] = topLeft;
            triangles[index++] = bottomRight;
            triangles[index++] = bottomLeft;

            triangles[index++] = topLeft;
            triangles[index++] = topRight;
            triangles[index++] = bottomRight;
        });


        var mesh = new MeshData {
            Vertices = vertices,
            Triangles = triangles,
            Uvs = uvs
        };
        return mesh;
    }
}

[Serializable]
public struct MeshData {
    public Vector3[] Vertices;
    public Vector2[] Uvs;
    public int[] Triangles;

    public UnityEngine.Mesh generateMesh() {
        var mesh = new UnityEngine.Mesh {vertices = Vertices, triangles = Triangles, uv = Uvs};
        mesh.RecalculateNormals();
        return mesh;
    }
}
}