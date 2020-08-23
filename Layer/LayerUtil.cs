using UnityEngine;

namespace Grimity.Layer {
public static class LayerUtil {
    public static bool Contains(this LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }

    public static int LayerMaskToLayer(LayerMask layerMask) {
        var layerNumber = 0;
        var layer = layerMask.value;
        while (layer > 0) {
            layer = layer >> 1;
            layerNumber++;
        }

        return layerNumber - 1;
    }
}
}