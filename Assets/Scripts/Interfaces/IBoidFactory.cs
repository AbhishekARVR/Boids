using UnityEngine;

/// <summary>
/// Interface for a factory that creates boids.
/// </summary>
public interface IBoidFactory
{
    /// <summary>
    /// Creates a new boid at the specified position.
    /// </summary>
    /// <param name="position">Spawn position.</param>
    /// <returns>The created boid.</returns>
    IBoid CreateBoid(Vector2 position);
}