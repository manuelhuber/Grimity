namespace Grimity.Int {
public class IntLoop {
    private int _max;
    private int _min;
    private int _current;

    public int Value {
        get => _current;
        set {
            if (value >= _min && value <= _max) {
                _current = value;
            }
        }
    }

    public IntLoop(int max, int min = 0) {
        _min = min;
        _max = max;
        _current = min;
    }

    public void UpdateMax(int max) {
        _max = max;
        if (_current > max) {
            _current = _min;
        }
    }

    public int Next() {
        _current++;
        if (_current > _max) {
            _current = _min;
        }

        return _current;
    }

    public int Previous() {
        _current--;
        if (_current < _min) {
            _current = _max;
        }

        return _current;
    }
}
}