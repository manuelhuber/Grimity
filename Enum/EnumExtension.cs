namespace Grimity.Enum {
public static class EnumExtension {
    public static T ToEnum<T>(this string value, T defaultValue) where T : struct {
        if (string.IsNullOrEmpty(value)) {
            return defaultValue;
        }

        return System.Enum.TryParse<T>(value, true, out var result) ? result : defaultValue;
    }

    public static T ToEnum<T>(this string value) {
        return (T) System.Enum.Parse(typeof(T), value, true);
    }
}
}