using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Grimity.Localization {
/// <summary>
///     A smart string variable holding a list of variables, written with Smart Format's list formatter:
///     <c>{name:list:{}|, |, and }</c>
/// </summary>
public class ListVariable : IVariable {
    private readonly IReadOnlyList<IVariable> _items;

    public ListVariable(IReadOnlyList<IVariable> items) {
        _items = items;
    }

    // Resolved on every format, so nested localized strings come out in the locale being formatted
    public object GetSourceValue(ISelectorInfo selector) {
        foreach (var item in _items) selector.UpdateOnChangeOf(item);
        return _items.Select(item => item.GetSourceValue(selector)).ToList();
    }
}
}