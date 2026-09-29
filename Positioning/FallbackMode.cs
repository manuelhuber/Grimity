namespace Grimity.Positioning {
/// <summary>What to try when the preferred placement doesn't fit.</summary>
public enum FallbackMode {
    /// <summary>Always use the preferred placement.</summary>
    None,

    /// <summary>Try the opposite side.</summary>
    Flip,

    /// <summary>Try the fallback list in order.</summary>
    Custom,

    /// <summary>Use the side with the most space, keeping the preferred alignment.</summary>
    Auto
}
}
