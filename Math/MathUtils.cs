using UnityEngine;

namespace Grimity.Math {
public static class MathUtils {
    /// <summary>
    /// Rounds the given number to the nearest multiple of the given number.
    /// E.g. multiple=4 would round numbers to ..., -4, 0, 4, 8, 12, ...
    /// with offset by half we would round to -4, -2, 2, 6, 10, ...
    /// Numbers that are the exact middle between 2 multiple well be rounded down.
    /// </summary>
    /// <param name="numberRaw">The number to round</param>
    /// <param name="multiple">The number to whose multiple we should round</param>
    /// <param name="offsetByHalf">Round to the numbers halfway between the multiples</param>
    /// <returns></returns>
    public static int RoundToMultiple(float numberRaw, int multiple, bool offsetByHalf) {
        var negative = numberRaw < 0;
        var number = Mathf.Abs(numberRaw);
        var modulo = number % multiple;
        float x;
        if (offsetByHalf) {
            var roundDown = modulo > multiple * .5f;
            var half = multiple / 2f;
            x = roundDown ? number - (modulo - half) : number + (half - modulo);
        } else {
            var roundDown = modulo < multiple * .5f;
            x = roundDown ? number - modulo : number + (multiple - modulo);
        }

        return (negative ? -1 : 1) * Mathf.RoundToInt(x);
    }
}
}