using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Grimity.Editor {
public class EditorAssetUtil {
    public static T[] GetAll<T>() where T : Object {
        return AssetDatabase.FindAssets($"t:{typeof(T).Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<T>)
            .Where(asset => asset)
            .ToArray();
    }
}
}