using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grimity.Data {
[Serializable]
public struct Sides {
    [HorizontalGroup("1")] public float Top;
    [HorizontalGroup("1")] public float Bottom;
    [HorizontalGroup("1")] public float Left;
    [HorizontalGroup("1")] public float Right;

    public static implicit operator RectOffset(Sides sides) {
        return new RectOffset((int)sides.Left, (int)sides.Right, (int)sides.Top, (int)sides.Bottom);
    }
}
}