using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grimity.UserInput {
public static class InputUtils {
    private static readonly KeyCode[] KeyboardAndMouseKeyCodes =
        System.Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Where(k => (int)k <= (int)KeyCode.Mouse6)
            .ToArray();

    public static HashSet<KeyCode> GetCurrentKeysDown() {
        return !Input.anyKeyDown
            ? new HashSet<KeyCode>()
            : KeyboardAndMouseKeyCodes.Where(Input.GetKeyDown).ToHashSet();
    }

    public static HashSet<KeyCode> GetCurrentKeys() {
        return !Input.anyKey
            ? new HashSet<KeyCode>()
            : KeyboardAndMouseKeyCodes.Where(Input.GetKey).ToHashSet();
    }

    public static HashSet<KeyCode> GetCurrentKeysUp() {
        return KeyboardAndMouseKeyCodes.Where(Input.GetKeyUp).ToHashSet();
    }
}
}