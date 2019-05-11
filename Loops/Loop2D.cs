using System;

namespace Grimity.Loops {
class Loop2D {
    private readonly int _height;
    private readonly int _width;

    public Loop2D(int width, int height) {
        _height = height;
        _width = width;
    }

    public Loop2D(int squareSize) {
        _height = _width = squareSize;
    }

    public void loopByColumn(Action<int, int> action) {
        for (var x = 0; x < _width; x++) {
            for (var y = 0; y < _height; y++) {
                action(x, y);
            }
        }
    }

    public void loopByRow(Action<int, int> action) {
        for (var y = 0; y < _height; y++) {
            for (var x = 0; x < _width; x++) {
                action(x, y);
            }
        }
    }
}
}