using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Interface for flocking behavior rules that affect boid movement.
/// </summary>
public interface IFlockingRule
{
    /// <summary>
    /// Calculate the steering force based on the rule for a specific boid.
    /// </summary>
    /// <param name="boid">The boid to calculate for.</param>
    /// <param name="neighbors">List of neighboring boids.</param>
    /// <returns>Calculated steering force vector.</returns>
    Vector2 CalculateForce(IBoid boid, IReadOnlyList<IBoid> neighbors);
    
    /// <summary>
    /// Gets or sets the weight of this rule.
    /// </summary>
    float Weight { get; set; }
}