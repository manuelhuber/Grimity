using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grimity.UserInput {
public static class InputUtils {
    private static readonly KeyCode[] KeyboardKeyCodes =
        System.Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Where(k => (int) k <= (int) KeyCode.Mouse0)
            .ToArray();

    public static IEnumerable<KeyCode> GetCurrentKeysDown() {
        if (!Input.anyKeyDown) yield break;
        foreach (var t in KeyboardKeyCodes) {
            if (Input.GetKeyDown(t)) {
                yield return t;
            }
        }
    }

    public static IEnumerable<KeyCode> GetCurrentKeys() {
        if (!Input.anyKey) yield break;
        foreach (var t in KeyboardKeyCodes) {
            if (Input.GetKey(t)) {
                yield return t;
            }
        }
    }

    public static IEnumerable<KeyCode> GetCurrentKeysUp() {
        return KeyboardKeyCodes.Where(Input.GetKeyUp);
    }
}
}