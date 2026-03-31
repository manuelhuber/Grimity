using Random = UnityEngine.Random;

namespace Grimity.Enum {
public static class EnumExtension {
    public static T ToEnum<T>(this string value, T defaultValue) where T : struct {
        if (string.IsNullOrEmpty(value)) {
            return defaultValue;
        }

        return System.Enum.TryParse<T>(value, true, out var result) ? result : defaultValue;
    }

    public static T ToEnum<T>(this string value) {
        return (T)System.Enum.Parse(typeof(T), value, true);
    }

    public static T GetRandom<T>() where T : struct, System.Enum {
        var values = (T[])System.Enum.GetValues(typeof(T));
        return values[Random.Range(0, values.Length)];
    }
}
}