using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Grimity.Localization {
/// <summary>A smart string variable whose value never changes, e.g. a number taken from a definition asset</summary>
public class ConstantVariable : IVariable {
    private readonly object _value;

    public ConstantVariable(object value) {
        _value = value;
    }

    public object GetSourceValue(ISelectorInfo selector) => _value;
}
}