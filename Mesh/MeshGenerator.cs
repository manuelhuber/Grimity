using Grimity.Loops;
using UnityEngine;

namespace Grimity.Mesh {
public class MeshGenerator {
    public static UnityEngine.Mesh GenerateMesh(int height,
                                                int width,
                                                float[,] heightMap,
                                                float heightAmplifier,
                                                AnimationCurve heightCurve,
                                                float distanceBetweenVertices = 1f) {
        // VERTICES & UVS ------------
        var vertices = new Vector3[height * width];
        var uvs = new Vector2[height * width];
        var index = 0;

        new Loop2D(width, height).loopY((x, y) => {
            var elevation = heightCurve.Evaluate(heightMap[x, y]) * heightAmplifier;
            vertices[index] = new Vector3(x * distanceBetweenVertices, elevation, y * distanceBetweenVertices);
            uvs[index] = new Vector2(x / (float) width, y / (float) height);
            index++;
        });

        // TRIANGLES ------------
        var triangles = new int[3 * 2 * (width - 1) * (height - 1)];
        index = 0;
        new Loop2D(width - 1, height - 1).loopX((x, y) => {
            var topLeft = height * x + y;
            var topRight = topLeft + 1;
            var bottomRight = topRight + height;
            var bottomLeft = topLeft + height;
            triangles[index++] = topLeft;
            triangles[index++] = bottomRight;
            triangles[index++] = bottomLeft;

            triangles[index++] = topLeft;
            triangles[index++] = topRight;
            triangles[index++] = bottomRight;
        });


        var mesh = new UnityEngine.Mesh {vertices = vertices, triangles = triangles, uv = uvs};
        mesh.RecalculateNormals();
        return mesh;
    }
}
}