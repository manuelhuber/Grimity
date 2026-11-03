using System;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Grimity.Localization {
/// <summary>A smart string variable that shows the text of another variable formatted, e.g. colored</summary>
public class FormattedVariable : IVariable {
    private readonly Func<string, string> _formatter;
    private readonly IVariable _variable;

    public FormattedVariable(IVariable variable, Func<string, string> formatter) {
        _variable = variable;
        _formatter = formatter;
    }

    public object GetSourceValue(ISelectorInfo selector) {
        selector.UpdateOnChangeOf(_variable);
        return _formatter(_variable.GetSourceValue(selector).ToString());
    }
}
}