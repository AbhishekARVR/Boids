using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main implementation of boid behavior.
/// </summary>
[RequireComponent(typeof(Transform))]
public class BoidBehavior : MonoBehaviour, IBoid
{
    // Settings reference
    private BoidSettings settings;
    
    // Behavior state
    private Vector2 velocity;
    private Vector2 acceleration;
    private Transform cachedTransform;
    
    // Flocking behaviors
    private List<IFlockingRule> flockingRules = new List<IFlockingRule>();
    
    // Boundary reference
    private IBoundary boundary;

    // Debug visualization
    [SerializeField] private bool showDebugGizmos = false;
    private Vector2[] debugForces = new Vector2[4]; // cohesion, alignment, separation, boundary
    
    /// <summary>
    /// Initialize the boid with required dependencies.
    /// </summary>
    public void Initialize(BoidSettings settings, IBoundary boundary, IEnumerable<IFlockingRule> rules)
    {
        this.settings = settings;
        this.boundary = boundary;
        
        cachedTransform = transform;
        
        // Add all provided rules
        flockingRules.Clear();
        foreach (var rule in rules)
        {
            flockingRules.Add(rule);
        }
        
        // Set random initial velocity
        float startSpeed = Random.Range(0.5f * settings.MaxSpeed, settings.MaxSpeed);
        velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * startSpeed;
        
        // Set initial rotation based on velocity
        UpdateRotation();
    }
    
    /// <summary>
    /// Updates the boid's state for the current frame.
    /// </summary>
    public void UpdateBoid(float deltaTime)
    {
        // Apply velocity to position
        cachedTransform.position += (Vector3)velocity * deltaTime;
        
        // Apply acceleration to velocity
        velocity += acceleration * deltaTime;
        
        // Limit speed
        if (velocity.magnitude > settings.MaxSpeed)
        {
            velocity = velocity.normalized * settings.MaxSpeed;
        }
        else if (velocity.magnitude < settings.MaxSpeed * 0.5f)
        {
            // Ensure minimum speed to prevent stalling
            velocity = velocity.normalized * settings.MaxSpeed * 0.5f;
        }
        
        // Reset acceleration for next frame
        acceleration = Vector2.zero;
        
        // Update rotation based on velocity
        UpdateRotation();
    }
    
    /// <summary>
    /// Apply flocking rules and update acceleration.
    /// </summary>
    public void ApplyFlockingBehavior(IReadOnlyList<IBoid> neighbors)
    {
        int ruleIndex = 0;
        foreach (var rule in flockingRules)
        {
            Vector2 force = rule.CalculateForce(this, neighbors);
            
            // Store for debugging
            if (ruleIndex < 3 && showDebugGizmos)
            {
                debugForces[ruleIndex] = force;
            }
            
            ApplyForce(force);
            ruleIndex++;
        }
        
        // Apply boundary force if a boundary is set
        if (boundary != null)
        {
            Vector2 boundaryForce = boundary.CalculateSteeringForce(this) * settings.BoundaryWeight;
            
            // Store for debugging
            if (showDebugGizmos)
            {
                debugForces[3] = boundaryForce;
            }
            
            ApplyForce(boundaryForce);
        }
    }
    
    /// <summary>
    /// Applies a steering force to the boid.
    /// </summary>
    public void ApplyForce(Vector2 force)
    {
        // Skip if force is negligible
        if (force.sqrMagnitude < 0.001f)
            return;
            
        acceleration += force;
        
        // Limit acceleration to max steer force
        if (acceleration.magnitude > settings.MaxSteerForce)
        {
            acceleration = acceleration.normalized * settings.MaxSteerForce;
        }
    }
    
    /// <summary>
    /// Update rotation to match velocity direction.
    /// </summary>
    private void UpdateRotation()
    {
        if (velocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            cachedTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    // For debugging
    private void OnDrawGizmos()
    {
        if (!showDebugGizmos || !Application.isPlaying)
            return;
            
        // Draw velocity
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, velocity.normalized);
        
        // Draw acceleration
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, acceleration.normalized * 0.5f);
        
        // Draw rule forces
        Gizmos.color = Color.blue; // Cohesion
        Gizmos.DrawRay(transform.position, debugForces[0].normalized * 0.3f);
        
        Gizmos.color = Color.yellow; // Alignment
        Gizmos.DrawRay(transform.position, debugForces[1].normalized * 0.3f);
        
        Gizmos.color = Color.magenta; // Separation
        Gizmos.DrawRay(transform.position, debugForces[2].normalized * 0.3f);
        
        Gizmos.color = Color.cyan; // Boundary
        Gizmos.DrawRay(transform.position, debugForces[3].normalized * 0.3f);
    }
    
    // Properties
    public Vector2 Position => cachedTransform.position;
    public Vector2 Velocity => velocity;
}