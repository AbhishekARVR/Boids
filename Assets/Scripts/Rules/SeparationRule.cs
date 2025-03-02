using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of the separation flocking rule (steer away from nearby boids).
/// </summary>
public class SeparationRule : IFlockingRule
{
    private readonly BoidSettings settings;
    
    public float Weight { get; set; }
    
    public SeparationRule(BoidSettings settings, float weight)
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
        
        Vector2 steeringForce = Vector2.zero;
        int count = 0;
        
        foreach (var neighbor in neighbors)
        {
            if (neighbor == boid)
                continue;
                
            float distance = Vector2.Distance(boid.Position, neighbor.Position);
            
            if (distance < settings.AvoidanceRadius && distance > 0.001f)
            {
                // Calculate repulsion vector (away from neighbor)
                Vector2 repulsionVector = (boid.Position - neighbor.Position).normalized;
                
                // Scale by distance (closer boids have more influence)
                // Inverse square law creates stronger avoidance for closer boids
                float factor = 1.0f / (distance * distance);
                repulsionVector *= factor;
                
                steeringForce += repulsionVector;
                count++;
            }
        }
        
        if (count == 0)
        {
            return Vector2.zero;
        }
        
        // No need to average with inverse square law - stronger forces for closer neighbors
        
        // Scale to max speed
        if (steeringForce.magnitude > 0.001f)
        {
            steeringForce = steeringForce.normalized * settings.MaxSpeed;
            
            // Subtract current velocity for proper steering
            steeringForce -= boid.Velocity;
            
            // Limit to max steer force
            if (steeringForce.magnitude > settings.MaxSteerForce)
            {
                steeringForce = steeringForce.normalized * settings.MaxSteerForce;
            }
        }
        
        // Apply weight
        return steeringForce * Weight;
    }
}