using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Grimity.Localization {
/// <summary>
///     Named smart string variables. Added to a <see cref="UnityEngine.Localization.LocalizedString" /> under a name, its
///     variables are reachable as <c>{name.variable}</c>.
/// </summary>
/// <remarks>
///     Deliberately not an <see cref="IDictionary{TKey,TValue}" />: Smart Format's dictionary source would then return
///     the <see cref="IVariable" /> itself instead of its value.
/// </remarks>
public class VariableGroup : IVariable, IVariableGroup {
    private readonly Dictionary<string, IVariable> _variables = new();

    public IVariable this[string key] {
        set => _variables[key] = value;
    }

    public object GetSourceValue(ISelectorInfo selector) => this;

    public bool TryGetValue(string key, out IVariable value) => _variables.TryGetValue(key, out value);

    public void CopyTo(IDictionary<string, IVariable> target) {
        foreach (var variable in _variables) target[variable.Key] = variable.Value;
    }

    public void CopyTo(VariableGroup target) => CopyTo(target._variables);
}
}