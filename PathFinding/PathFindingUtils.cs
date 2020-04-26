using Unity.Mathematics;

namespace Grimity.PathFinding {
public static class PathFindingUtils {
    public static int ManhattanDistanceDiagonal(int fromX,
                                                int fromZ,
                                                int toX,
                                                int toZ,
                                                int moveStraightCost,
                                                int moveDiagonalCost) {
        var xDistance = math.abs(fromX - toX);
        var yDistance = math.abs(fromZ - toZ);
        var remaining = math.abs(xDistance - yDistance);
        return moveDiagonalCost * math.min(xDistance, yDistance) + moveStraightCost * remaining;
    }
}
}