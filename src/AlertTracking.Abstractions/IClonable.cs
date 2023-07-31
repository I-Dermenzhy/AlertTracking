namespace AlertTracking.Abstractions;

/// <summary>
/// Defines an interface for cloning objects of the implementing type.
/// </summary>
/// <typeparam name="TSelf">The type of the object being cloned.</typeparam>
public interface IClonable<TSelf>
{
    /// <summary>
    /// Creates a new instance that is a deep copy of the current object.
    /// </summary>
    /// <returns>A new instance of the implementing type that is a copy of the current object.</returns>
    TSelf Clone();
}
