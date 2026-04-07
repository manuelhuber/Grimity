using UnityEngine;

namespace Grimity.Rng {
public static class RandomUtils {
    public static bool Flip(float chance = .5f) {
        return Random.value < chance;
    }

    public static int MaybeNegative(int num) {
        return (Flip() ? 1 : -1) * num;
    }

    public static float MaybeNegative(this float num) {
        return (Flip() ? 1 : -1) * num;
    }
}
}