using UnityEngine.Localization;

namespace Grimity.Localization {
public static class LocalizationExtensions {
    public static bool IsEmpty(this LocalizedString localizedString) {
        return localizedString == null || localizedString.IsEmpty;
    }
}
}