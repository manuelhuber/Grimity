using System;
using UnityEngine;

namespace Grimity.Tooltip {
public abstract class TooltipView : MonoBehaviour {
    public abstract Type DataType { get; }
    public abstract void Populate(TooltipData data);
}

public abstract class TooltipView<T> : TooltipView where T : TooltipData {
    public override Type DataType => typeof(T);
    public override void Populate(TooltipData data) => TypedPopulate((T)data);
    protected abstract void TypedPopulate(T data);
}
}