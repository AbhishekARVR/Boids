using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Interface for boid entities in a flocking simulation.
/// </summary>
public interface IBoid
{
    /// <summary>
    /// Gets the current position of the boid.
    /// </summary>
    Vector2 Position { get; }
    
    /// <summary>
    /// Gets the current velocity of the boid.
    /// </summary>
    Vector2 Velocity { get; }
    
    /// <summary>
    /// Updates the boid's state for the current frame.
    /// </summary>
    void UpdateBoid(float deltaTime);
    
    /// <summary>
    /// Applies a steering force to the boid.
    /// </summary>
    /// <param name="force">The force vector to apply.</param>
    void ApplyForce(Vector2 force);
}