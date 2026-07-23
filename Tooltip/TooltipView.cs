using System;
using Grimity.MonoBehaviours;

namespace Grimity.Tooltip {
public abstract class TooltipView : BetterBehaviour, IDisposable {
    private TooltipData _data;
    public abstract Type DataType { get; }

    public void Dispose() {
        // We use Dispose since the View might never be active and thus onDestroy isn't called
        if (_data != null) _data.OnRefresh -= Populate;
        Cleanup();
    }

    public void Bind(TooltipData data) {
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