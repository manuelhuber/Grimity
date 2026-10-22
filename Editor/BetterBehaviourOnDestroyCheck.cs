using System;
using System.Reflection;
using Grimity.MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace Grimity.Editor {
/// <summary>
///     Unity sends OnDestroy only to the most derived declaration, so a subclass that declares its own hides
///     BetterBehaviour's and skips all registered cleanup. The compiler only warns about the hiding.
/// </summary>
[InitializeOnLoad]
public static class BetterBehaviourOnDestroyCheck {
    private const BindingFlags DeclaredInstanceMethods =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    static BetterBehaviourOnDestroyCheck() {
        foreach (var type in TypeCache.GetTypesDerivedFrom<BetterBehaviour>()) {
            if (type.GetMethod("OnDestroy", DeclaredInstanceMethods, null, Type.EmptyTypes, null) == null) {
                continue;
            }

            Debug.LogError(
                $"{type.FullName} declares OnDestroy, which hides {nameof(BetterBehaviour)}'s and skips its cleanup. " +
                "Override OnDestroyed or use RegisterCleanup instead.");
        }
    }
}
}