using System;

namespace Grimity.Tooltip {
public class TooltipData {
    public event Action<TooltipData> OnRefresh;
    public void Refresh() => OnRefresh?.Invoke(this);
}
}