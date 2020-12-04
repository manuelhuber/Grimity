using UnityEngine;

namespace Grimity.Rng {
public static class RandomUtils {
    public static bool Flip() {
        return Random.value > .5f;
    }

    public static int MaybeNegative(int num) {
        return (Flip() ? 1 : -1) * num;
    }
}
}