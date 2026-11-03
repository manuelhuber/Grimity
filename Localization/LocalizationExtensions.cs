using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Grimity.Localization {
public static class LocalizationExtensions {
    public static bool IsEmpty(this LocalizedString localizedString) {
        return localizedString == null || localizedString.IsEmpty;
    }

    public static LocalizedString Copy(this LocalizedString localizedString) {
        return new LocalizedString(localizedString.TableReference, localizedString.TableEntryReference);
    }

    /// <summary>
    ///     Formats the string being formatted again when <paramref name="variable" /> changes, e.g. when a nested
    ///     localized string has finished loading. Smart Format does this only for the variables it looks up itself.
    /// </summary>
    public static void UpdateOnChangeOf(this ISelectorInfo selector, IVariable variable) {
        var formatCache = selector.FormatDetails.FormatCache;
        if (formatCache == null || variable is not IVariableValueChanged valueChanged) return;
        if (!formatCache.VariableTriggers.Contains(valueChanged)) formatCache.VariableTriggers.Add(valueChanged);
    }
}
}