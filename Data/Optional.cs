using System;

namespace Grimity.Data {
public abstract class Optional<T> {
    public abstract T Value { get; }
    public abstract bool HasValue { get; }

    public static Optional<T> NoValue() {
        return new NullOptional<T>();
    }

    public static Optional<T> Of(T value) {
        return new ValueOptional<T>(value);
    }
}

public static class OptionalExtensions {
    public static Optional<T> AsOptional<T>(this T me) {
        return me == null ? Optional<T>.NoValue() : Optional<T>.Of(me);
    }
}

internal class ValueOptional<T> : Optional<T> {
    public ValueOptional(T value) {
        Value = value;
    }

    public override T Value { get; }

    public override bool HasValue => true;
}

internal class NullOptional<T> : Optional<T> {
    public override T Value => throw new NoValue();
    public override bool HasValue => false;
}

internal class NoValue : Exception {
}
}