using UnityEngine;

/// <summary>
/// Contains configurable settings for boid behavior.
/// </summary>
[CreateAssetMenu(fileName = "BoidSettings", menuName = "Boid System/Boid Settings")]
public class BoidSettings : ScriptableObject
{
    [Header("Movement Settings")]
    [Tooltip("Maximum speed a boid can travel")]
    [Range(1f, 10f)]
    public float MaxSpeed = 3.5f;
    
    [Tooltip("Maximum steering force that can be applied")]
    [Range(1f, 10f)]
    public float MaxSteerForce = 2.5f;
    
    [Header("Perception Settings")]
    [Tooltip("How far the boid can see other boids")]
    [Range(1f, 10f)]
    public float PerceptionRadius = 3.5f;
    
    [Tooltip("Minimum distance boids try to maintain from each other")]
    [Range(0.5f, 3f)]
    public float AvoidanceRadius = 1.25f;
    
    [Header("Rule Weights")]
    [Range(0f, 5f)]
    [Tooltip("Weight for cohesion rule - attraction to center of flock")]
    public float CohesionWeight = 1.2f;
    
    [Range(0f, 5f)]
    [Tooltip("Weight for alignment rule - match velocity with neighbors")]
    public float AlignmentWeight = 1.5f;
    
    [Range(0f, 5f)]
    [Tooltip("Weight for separation rule - avoid collisions with neighbors")]
    public float SeparationWeight = 2.0f;
    
    [Range(0f, 5f)]
    [Tooltip("Weight for boundary avoidance")]
    public float BoundaryWeight = 2.5f;
}