using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of the alignment flocking rule (steer towards average direction of flock).
/// </summary>
public class AlignmentRule : IFlockingRule
{
    private readonly BoidSettings settings;
    
    public float Weight { get; set; }
    
    public AlignmentRule(BoidSettings settings, float weight)
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
        
        Vector2 averageVelocity = Vector2.zero;
        int count = 0;
        
        foreach (var neighbor in neighbors)
        {
            float distance = Vector2.Distance(boid.Position, neighbor.Position);
            
            if (distance < settings.PerceptionRadius)
            {
                averageVelocity += neighbor.Velocity;
                count++;
            }
        }
        
        if (count == 0)
        {
            return Vector2.zero;
        }
        
        averageVelocity /= count;
        
        // Create desired velocity in the average direction
        Vector2 desiredVelocity = averageVelocity.normalized * settings.MaxSpeed;
        
        // Steering = desired - current velocity
        Vector2 steeringForce = desiredVelocity - boid.Velocity;
        
        // Apply weight
        return steeringForce * Weight;
    }
}