using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main manager for the boid flocking simulation.
/// </summary>
public class FlockSimulation : MonoBehaviour
{
    [Header("Boid Settings")]
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private BoidSettings boidSettings;
    [SerializeField] private int boidsCount = 50;
    
    [Header("Spawn Settings")]
    [SerializeField] private Vector2 spawnBounds = new Vector2(10f, 6f);
    [SerializeField] private bool useRandomSpawn = true;
    [SerializeField] private bool spawnInFormation = false;
    
    [Header("Boundary Settings")]
    [SerializeField] private Vector2 boundarySize = new Vector2(16f, 9f);
    [SerializeField] private float boundaryPadding = 0.05f;
    [SerializeField] private float boundaryBounceStrength = 3f;
    [SerializeField] private bool showBoundary = true;

    [Header("Debug")]
    [SerializeField] private bool showNeighborLines = false;
    [SerializeField] private Color neighborLineColor = Color.yellow;
    
    // Cached references
    private List<IBoid> boids = new List<IBoid>();
    private IBoidFactory boidFactory;
    private IBoundary boundary;
    
    private void Start()
    {
        InitializeSimulation();
        SpawnBoids();
    }
    
    private void InitializeSimulation()
    {
        // Create boundary
        boundary = new RectangularBoundary(
            transform.position,
            boundarySize,
            boundaryPadding,
            boundaryBounceStrength
        );
        
        // Create boid factory
        boidFactory = new BoidFactory(boidPrefab, boidSettings, boundary, transform);
    }
    
    private void SpawnBoids()
    {
        boids.Clear();
        
        if (spawnInFormation)
        {
            // Spawn boids in a tighter formation to encourage flocking
            Vector2 center = transform.position;
            float radius = Mathf.Min(spawnBounds.x, spawnBounds.y) * 0.25f;
            
            for (int i = 0; i < boidsCount; i++)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float distance = Random.Range(0, radius);
                Vector2 offset = new Vector2(
                    Mathf.Cos(angle) * distance,
                    Mathf.Sin(angle) * distance
                );
                
                Vector2 spawnPosition = center + offset;
                
                // Create boid through factory
                IBoid boid = boidFactory.CreateBoid(spawnPosition);
                boids.Add(boid);
            }
        }
        else
        {
            for (int i = 0; i < boidsCount; i++)
            {
                Vector2 spawnPosition = CalculateSpawnPosition(i);
                
                // Create boid through factory
                IBoid boid = boidFactory.CreateBoid(spawnPosition);
                boids.Add(boid);
            }
        }
    }
    
    private Vector2 CalculateSpawnPosition(int index)
    {
        if (useRandomSpawn)
        {
            // Random position within spawn bounds
            return (Vector2)transform.position + new Vector2(
                Random.Range(-spawnBounds.x / 2, spawnBounds.x / 2),
                Random.Range(-spawnBounds.y / 2, spawnBounds.y / 2)
            );
        }
        else
        {
            // Grid pattern
            int rows = Mathf.CeilToInt(Mathf.Sqrt(boidsCount));
            int cols = Mathf.CeilToInt((float)boidsCount / rows);
            
            float rowSpacing = spawnBounds.y / rows;
            float colSpacing = spawnBounds.x / cols;
            
            int row = index / cols;
            int col = index % cols;
            
            return (Vector2)transform.position + new Vector2(
                (col - cols / 2) * colSpacing + colSpacing / 2,
                (row - rows / 2) * rowSpacing + rowSpacing / 2
            );
        }
    }
    
    private void Update()
    {
        // Crucially important: calculate all forces BEFORE applying any movement
        // First apply all flocking behaviors to calculate forces
        foreach (var boid in boids)
        {
            if (boid is BoidBehavior boidBehavior)
            {
                boidBehavior.ApplyFlockingBehavior(boids);
            }
        }
        
        // Then update all boids' positions
        foreach (var boid in boids)
        {
            boid.UpdateBoid(Time.deltaTime);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (showBoundary)
        {
            // Draw boundary
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(boundarySize.x, boundarySize.y, 0));
            
            // Draw spawn area
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(spawnBounds.x, spawnBounds.y, 0));
        }
        
        // Draw neighbor connections for debugging
        if (Application.isPlaying && showNeighborLines && boids != null && boidSettings != null)
        {
            Gizmos.color = neighborLineColor;
            
            foreach (var boid in boids)
            {
                foreach (var other in boids)
                {
                    if (boid != other)
                    {
                        float distance = Vector2.Distance(boid.Position, other.Position);
                        if (distance < boidSettings.PerceptionRadius)
                        {
                            // Draw a line between neighbors with alpha based on distance
                            float alpha = 1 - (distance / boidSettings.PerceptionRadius);
                            Gizmos.color = new Color(neighborLineColor.r, neighborLineColor.g, neighborLineColor.b, alpha * 0.5f);
                            Gizmos.DrawLine(boid.Position, other.Position);
                        }
                    }
                }
            }
        }
    }
    
    // Public API for extensibility
    
    /// <summary>
    /// Gets a read-only list of all boids in the simulation.
    /// </summary>
    public IReadOnlyList<IBoid> GetBoids()
    {
        return boids.AsReadOnly();
    }
    
    /// <summary>
    /// Adds a new boid to the simulation at the specified position.
    /// </summary>
    public IBoid AddBoid(Vector2 position)
    {
        IBoid boid = boidFactory.CreateBoid(position);
        boids.Add(boid);
        return boid;
    }
    
    /// <summary>
    /// Gets the boundary for this simulation.
    /// </summary>
    public IBoundary GetBoundary()
    {
        return boundary;
    }
    
    /// <summary>
    /// Resets the simulation with the current settings.
    /// </summary>
    public void ResetSimulation()
    {
        // Clean up existing boids
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
            {
                Destroy(child.gameObject);
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }
        }
        
        boids.Clear();
        
        // Respawn boids
        SpawnBoids();
    }
}