using UnityEngine;

/// <summary>
/// Implementation of a rectangular boundary for containing boids.
/// </summary>
public class RectangularBoundary : IBoundary
{
    private readonly Bounds boundingBox;
    private readonly float padding;
    private readonly float bounceStrength;
    
    public Bounds Bounds => boundingBox;
    
    /// <summary>
    /// Creates a rectangular boundary.
    /// </summary>
    /// <param name="center">Center of the boundary.</param>
    /// <param name="size">Size of the boundary.</param>
    /// <param name="padding">Distance from edge where force starts.</param>
    /// <param name="bounceStrength">Strength of the repelling force.</param>
    public RectangularBoundary(Vector2 center, Vector2 size, float padding, float bounceStrength)
    {
        boundingBox = new Bounds(center, new Vector3(size.x, size.y, 1));
        this.padding = padding;
        this.bounceStrength = bounceStrength;
    }
    
    public bool IsInside(Vector2 position)
    {
        return boundingBox.Contains(position);
    }
    
    public Vector2 CalculateSteeringForce(IBoid boid)
    {
        Vector2 steeringForce = Vector2.zero;
        Vector2 position = boid.Position;
        
        // Calculate padding as a percentage of the boundary size
        float horizontalPadding = boundingBox.size.x * padding;
        float verticalPadding = boundingBox.size.y * padding;
        
        // Create a soft boundary force that increases as the boid approaches the edge
        if (position.x < boundingBox.min.x + horizontalPadding)
        {
            float intensity = Mathf.InverseLerp(boundingBox.min.x, boundingBox.min.x + horizontalPadding, position.x);
            steeringForce.x += bounceStrength * (1 - intensity);
        }
        else if (position.x > boundingBox.max.x - horizontalPadding)
        {
            float intensity = Mathf.InverseLerp(boundingBox.max.x, boundingBox.max.x - horizontalPadding, position.x);
            steeringForce.x -= bounceStrength * (1 - intensity);
        }
        
        if (position.y < boundingBox.min.y + verticalPadding)
        {
            float intensity = Mathf.InverseLerp(boundingBox.min.y, boundingBox.min.y + verticalPadding, position.y);
            steeringForce.y += bounceStrength * (1 - intensity);
        }
        else if (position.y > boundingBox.max.y - verticalPadding)
        {
            float intensity = Mathf.InverseLerp(boundingBox.max.y, boundingBox.max.y - verticalPadding, position.y);
            steeringForce.y -= bounceStrength * (1 - intensity);
        }
        
        return steeringForce;
    }
}