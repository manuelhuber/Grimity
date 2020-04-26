using UnityEngine;

namespace Grimity.Layer {
public static class LayerUtil {
    public static bool Contains(this LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }
}
}