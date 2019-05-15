using System;

namespace Grimity.Loops {
class Loop2D {
    private readonly int _xMax;
    private readonly int _yMax;
    private readonly Func<int, int> _increment = (int x) => x + 1;

    public Loop2D(int yMax, int xMax) {
        _xMax = xMax;
        _yMax = yMax;
    }

    public Loop2D(int yMax, int xMax, Func<int, int> increment) {
        _xMax = xMax;
        _yMax = yMax;
        _increment = increment;
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
        for (var x = 0; x < _yMax; x = _increment(x)) {
            for (var y = 0; y < _xMax; y = _increment(y)) {
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
        for (var y = 0; y < _xMax; y = _increment(y)) {
            for (var x = 0; x < _yMax; x = _increment(x)) {
                action(x, y);
            }
        }
    }
}
}