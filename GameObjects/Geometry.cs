using UnityEngine;

namespace Grimity.GameObjects {
public class Geometry {
    public static Vector3 LowerCenter(GameObject gameObject) {
        var meshFilter = gameObject.GetComponent<MeshFilter>();
        var bounds = meshFilter.sharedMesh.bounds;
        return bounds.center + new Vector3(0, -bounds.extents.y, 0);
    }
}
}