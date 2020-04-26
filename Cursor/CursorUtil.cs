using UnityEngine;

namespace Grimity.Cursor {
public class CursorUtil {
    /// <summary>
    ///     Casts a ray from the main camera through the cursor and outputs the first hit on the given layer mask
    /// </summary>
    /// <returns>true if terrain has been hit</returns>
    public static bool GetCursorLocation(out RaycastHit hit,
                                         Camera camera,
                                         int layerMask = -5,
                                         int maxDistance = 10000,
                                         bool debug = false) {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (debug) Debug.DrawRay(camera.transform.position, ray.direction * 1000, Color.red);
        return Physics.Raycast(ray, out hit, maxDistance, layerMask);
    }

    public static bool GetCursorLocation(out Vector3 hit,
                                         int layerMask,
                                         Camera camera) {
        var boo = GetCursorLocation(out var rayHit, camera, layerMask);
        hit = rayHit.point;
        return boo;
    }

    public static bool GetCursorLocation(out GameObject hit,
                                         int layerMask,
                                         Camera camera) {
        var boo = GetCursorLocation(out var rayHit, camera, layerMask);
        hit = rayHit.transform.gameObject;
        return boo;
    }
}
}