using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of the cohesion flocking rule (steer towards center of flock).
/// </summary>
public class CohesionRule : IFlockingRule
{
    private readonly BoidSettings settings;
    
    public float Weight { get; set; }
    
    public CohesionRule(BoidSettings settings, float weight)
    {
        this.settings = settings;
        this.Weight = weight;
    }
    
    public Vector2 CalculateForce(IBoid boid, IReadOnlyList<IBoid> neighbors)
    {
        if (neighbors.Count == 0)
        {
            return Vector2.zero;
        }
        
        Vector2 centerOfMass = Vector2.zero;
        int count = 0;
        
        foreach (var neighbor in neighbors)
        {
            float distance = Vector2.Distance(boid.Position, neighbor.Position);
            
            if (distance < settings.PerceptionRadius)
            {
                centerOfMass += neighbor.Position;
                count++;
            }
        }
        
        if (count == 0)
        {
            return Vector2.zero;
        }
        
        centerOfMass /= count;
        
        // Create desired velocity towards center of mass
        Vector2 desiredVelocity = (centerOfMass - boid.Position).normalized * settings.MaxSpeed;
        
        // Steering = desired - current velocity
        Vector2 steeringForce = desiredVelocity - boid.Velocity;
        
        // Apply weight
        return steeringForce * Weight;
    }
}