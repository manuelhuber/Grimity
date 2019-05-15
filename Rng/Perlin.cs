using System;
using Grimity.Loops;
using UnityEngine;
using Random = System.Random;

namespace Grimity.Rng {
public class Perlin {
    public static float[,] GeneratePerlinArray(int width,
                                               int height,
                                               int octaves,
                                               float scale,
                                               float persistence,
                                               float lacunarity,
                                               int seed,
                                               int offsetX,
                                               int offsetY) {
        var amplitude = 1f;
        var frequency = 1f;
        var rng = new Random(seed);
        var data = new float[width, height];

        float maxPossibleHeight = 0;
        for (var i = 0; i < octaves; i++) {
            var xOffset = rng.Next(-100000, 100000) + offsetX;
            var yOffset = rng.Next(-100000, 100000) + offsetY;

            maxPossibleHeight += amplitude;
            AddOctave(data, xOffset, yOffset, scale, amplitude, frequency);
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        new Loop2D(width, height).loopY((x, y) => {
            var normalizedHeight = (data[x, y]) / (maxPossibleHeight);
            data[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
        });


        return data;
    }

    public static void AddOctave(float[,] data,
                                 int xOffset,
                                 int yOffset,
                                 float scale,
                                 float amplitude,
                                 float frequency) {
        var width = data.GetLength(0);
        var height = data.GetLength(1);
        new Loop2D(width, height).loopY((x, y) => {
            data[x, y] += amplitude * PerlinValue(x, y, scale, xOffset, yOffset, frequency);
        });
    }

    private static float PerlinValue(int x, int y, float scale, int xOffset, int yOffset, float frequency) {
        var xSample = (x + xOffset) / scale * frequency;
        var ySample = (y + yOffset) / scale * frequency;
        var perlinNoise = Mathf.PerlinNoise(xSample, ySample);
        // Debug.Log($"{xSample}, {ySample} = {perlinNoise}");
        return perlinNoise;
    }
}
}