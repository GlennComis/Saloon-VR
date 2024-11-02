using UnityEngine;
using UnityEngine.Events;

public class PointLightTimer : MonoBehaviour
{
    public UnityEvent OnDecayComplete;
    
    [Header("Dependencies")]
    [SerializeField] private Light pointLight;
    [SerializeField] private SphereCollider sphereCollider;
    
    [Header("Configuration")]
    [SerializeField] private float decayDuration = 5f; // Time in seconds for the decay to complete
    [SerializeField] private float initialRange = 3f; // Starting range for both collider and light
    [SerializeField] private float initialIntensity = 0.5f; // Starting intensity for the light

    // State
    private float decayTimer = 0f;
    private bool decayComplete;
    
    private void Start()
    {
        // Set initial values
        pointLight.range = initialRange;
        pointLight.intensity = initialIntensity;
        sphereCollider.radius = initialRange;
    }

    private void Update()
    {
        if (decayComplete)
            return;
        
        if (decayTimer < decayDuration)
        {
            // Increment the decay timer
            decayTimer += Time.deltaTime;

            // Calculate the decay progress as a value between 1 (start) and 0 (end)
            float decayProgress = 1f - (decayTimer / decayDuration);

            // Apply decay to range and intensity
            pointLight.range = initialRange * decayProgress;
            pointLight.intensity = initialIntensity * decayProgress;
            sphereCollider.radius = initialRange * decayProgress;
        }
        else
        {
            // Once decay is complete, ensure values are set to 0
            pointLight.range = 0;
            pointLight.intensity = 0;
            sphereCollider.radius = 0;
            
            decayComplete = true;
            OnDecayComplete?.Invoke();

            // Optionally, destroy the GameObject if you want it to disappear
            // Destroy(gameObject);
        }
    }
}
