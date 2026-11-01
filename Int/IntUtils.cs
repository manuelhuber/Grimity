using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grimity.Int {
public static class IntUtils {
    public static int Clamp0(this int value) {
        return value < 0 ? 0 : value;
    }

    public static bool Within(this int value, Vector2Int range) {
        return value >= range.x && value <= range.y;
    }

    public static int GetRandomElement(this int2 range) {
        return Random.Range(range.x, range.y + 1);
    }
}
}