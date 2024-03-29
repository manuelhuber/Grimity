using System.Linq;
using System.Threading;

namespace Grimity.String {
public static class StringExtensions {
    public static bool Falsy(this string s) {
        return string.IsNullOrEmpty(s);
    }

    public static bool Truthy(this string s) {
        return !s.Falsy();
    }

    public static string ToPascalCase(this string s) {
        return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s).Replace(" ", string.Empty);
    }

    public static string ToSentence(this string s) {
        return string.Concat(s.Select(c => char.IsUpper(c) ? " " + c : c.ToString())).TrimStart(' ');
    }
}
}