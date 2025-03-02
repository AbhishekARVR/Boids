using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Factory for creating boid instances.
/// </summary>
public class BoidFactory : IBoidFactory
{
    private readonly GameObject boidPrefab;
    private readonly BoidSettings settings;
    private readonly IBoundary boundary;
    private readonly Transform parent;
    
    public BoidFactory(GameObject boidPrefab, BoidSettings settings, IBoundary boundary, Transform parent)
    {
        this.boidPrefab = boidPrefab;
        this.settings = settings;
        this.boundary = boundary;
        this.parent = parent;
    }
    
    public IBoid CreateBoid(Vector2 position)
    {
        GameObject boidObject = Object.Instantiate(boidPrefab, position, Quaternion.identity, parent);
        
        BoidBehavior boid = boidObject.GetComponent<BoidBehavior>();
        
        // Create flocking rules
        List<IFlockingRule> rules = new List<IFlockingRule>
        {
            new CohesionRule(settings, settings.CohesionWeight),
            new AlignmentRule(settings, settings.AlignmentWeight),
            new SeparationRule(settings, settings.SeparationWeight)
        };
        
        // Initialize the boid
        boid.Initialize(settings, boundary, rules);
        
        return boid;
    }
}