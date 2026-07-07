using System;

namespace Grimity.Tooltip {
public abstract class TooltipData : IDisposable {
    public virtual void Dispose() {
    }

    public event Action<TooltipData> OnRefresh;
    public void Refresh() => OnRefresh?.Invoke(this);
}
}