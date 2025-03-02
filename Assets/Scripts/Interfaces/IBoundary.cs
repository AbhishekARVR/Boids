using UnityEngine;

/// <summary>
/// Interface for defining a boundary that contains boids.
/// </summary>
public interface IBoundary
{
    /// <summary>
    /// Check if a position is within the boundary.
    /// </summary>
    /// <param name="position">Position to check.</param>
    /// <returns>True if position is inside the boundary.</returns>
    bool IsInside(Vector2 position);
    
    /// <summary>
    /// Calculate steering force to keep a boid inside the boundary.
    /// </summary>
    /// <param name="boid">The boid to calculate for.</param>
    /// <returns>Steering force to stay within boundary.</returns>
    Vector2 CalculateSteeringForce(IBoid boid);
    
    /// <summary>
    /// Gets the bounds of this boundary.
    /// </summary>
    Bounds Bounds { get; }
}