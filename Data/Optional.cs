using System;

namespace Grimity.Data {
public class Optional<T> {
    public T Value { get; set; }
    public bool HasValue => Value != null;

    public static Optional<T> NoValue() {
        return new NullOptional<T>();
    }

    public static Optional<T> Of(T value) {
        return new Optional<T> {Value = value};
    }
}

internal class NullOptional<T> : Optional<T> {
    public new T Value => throw new NoValue();
    public new bool HasValue => false;
}

internal class NoValue : Exception {
}
}