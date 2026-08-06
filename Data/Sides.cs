using System;
using Sirenix.OdinInspector;

namespace Grimity.Data {
[Serializable]
public struct Sides {
    [HorizontalGroup("1")] public float Top;
    [HorizontalGroup("1")] public float Bottom;
    [HorizontalGroup("1")] public float Left;
    [HorizontalGroup("1")] public float Right;
}
}