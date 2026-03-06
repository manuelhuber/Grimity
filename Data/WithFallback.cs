namespace Grimity.Data {
public class WithFallback<T> {
    public T Fallback { get; set; }
    public T Value { get; set; }

    public WithFallback(T fallback = default, T value = default) {
        Fallback = fallback;
        Value = value;
    }

    public T Get() {
        return Value ?? Fallback;
    }

    public static implicit operator T(WithFallback<T> withFallback) => withFallback.Get();
}
}