using UnityEngine;

/// <summary>
/// Controls the visual appearance of a boid.
/// </summary>
[RequireComponent(typeof(BoidBehavior))]
public class BoidVisualController : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Color boidColor = Color.white;
    [SerializeField] private float trailTime = 0.5f;
    [SerializeField] private bool useTrail = true;
    
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    
    private void Awake()
    {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        
        // Initialize appearance
        InitializeAppearance();
    }
    
    private void InitializeAppearance()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = boidColor;
        }
        
        if (trailRenderer != null)
        {
            trailRenderer.time = trailTime;
            trailRenderer.startColor = boidColor;
            trailRenderer.endColor = new Color(boidColor.r, boidColor.g, boidColor.b, 0);
            trailRenderer.enabled = useTrail;
        }
    }
    
    /// <summary>
    /// Sets the color of the boid and its trail.
    /// </summary>
    public void SetColor(Color color)
    {
        boidColor = color;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        
        if (trailRenderer != null)
        {
            trailRenderer.startColor = color;
            trailRenderer.endColor = new Color(color.r, color.g, color.b, 0);
        }
    }
    
    /// <summary>
    /// Enables or disables the trail effect.
    /// </summary>
    public void SetTrailActive(bool active)
    {
        useTrail = active;
        
        if (trailRenderer != null)
        {
            trailRenderer.enabled = active;
        }
    }
}