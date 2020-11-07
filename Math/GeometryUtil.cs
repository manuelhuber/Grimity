using UnityEngine;

namespace Grimity.Math {
public static class GeometryUtil {
    /// <summary>
    ///     Checks if the target is in the field of view of the given viewer.
    ///     Distance between viewer and target and obstacles are ignored.
    /// </summary>
    /// <param name="viewer">The position of the "viewer"</param>
    /// <param name="viewDirection">The direction in which the viewer looks</param>
    /// <param name="target">The target that might be in the field of view</param>
    /// <param name="filedOfView">The "view angle" in degrees</param>
    /// <returns></returns>
    public static bool IsInView(Vector3 viewer, Vector3 viewDirection, Vector3 target, float filedOfView) {
        var casterToTarget = (target - viewer).normalized;
        var rightAngle = filedOfView / 2;
        var leftAngle = 360 - rightAngle;
        var angle = Vector3.Angle(viewDirection, casterToTarget);
        var isInArc = angle <= rightAngle || angle >= leftAngle;
        return isInArc;
    }
}
}