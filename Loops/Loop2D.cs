using System;

namespace Grimity.Loops {
class Loop2D {
    private readonly int _xMax;
    private readonly int _yMax;

    public Loop2D(int yMax, int xMax) {
        _xMax = xMax;
        _yMax = yMax;
    }

    public Loop2D(int max) {
        _xMax = _yMax = max;
    }

    /// <summary>
    /// Loops through y fast
    /// (0,0) - (0,1) - (0,2) - ... - (1,0) - (1,1) - ...
    /// </summary>
    /// <param name="action"></param>
    public void loopY(Action<int, int> action) {
        for (var x = 0; x < _yMax; x++) {
            for (var y = 0; y < _xMax; y++) {
                action(x, y);
            }
        }
    }

    /// <summary>
    /// Loops through X fast
    /// (0,0) - (1,0) - (2,0) - ... - (0,1) - (1,1) - ...
    /// 
    /// </summary>
    /// <param name="action"></param>
    public void loopX(Action<int, int> action) {
        for (var y = 0; y < _xMax; y++) {
            for (var x = 0; x < _yMax; x++) {
                action(x, y);
            }
        }
    }
}
}