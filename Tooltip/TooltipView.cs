using System;
using UnityEngine;

namespace Grimity.Tooltip {
public abstract class TooltipView : MonoBehaviour {
    private TooltipData _data;
    public abstract Type DataType { get; }

    private void OnDestroy() {
        if (_data != null) _data.OnRefresh -= Populate;
    }

    public void Bind(TooltipData data) {
        if (_data != null) _data.OnRefresh -= Populate;
        _data = data;
        _data.OnRefresh += Populate;
        Populate(data);
    }

    public abstract void Populate(TooltipData data);
}

public abstract class TooltipView<T> : TooltipView where T : TooltipData {
    public override Type DataType => typeof(T);
    public override void Populate(TooltipData data) => TypedPopulate((T)data);
    protected abstract void TypedPopulate(T data);
}
}