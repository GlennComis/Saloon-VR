using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointLightTimer : MonoBehaviour
{
    public UnityEvent OnDecayComplete;
    
    [Header("Dependencies")]
    [SerializeField] private Light pointLight;
    [SerializeField] private SphereCollider sphereCollider;
    
    [Header("Optional Blendshape")]
    [SerializeField] private List<SkinnedMeshRenderer> skinnedMeshRenderers; // The renderer with the blendshape
    [SerializeField] private int blendShapeIndex = 0; // Index of the blendshape to modify
    
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
            Replenish();
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
            
            // Update blendshape weight from 0 to 100 based on decay progress
            if (skinnedMeshRenderers.Count > 0)
            {
                float blendShapeWeight = Mathf.Lerp(0, 100, 1f - decayProgress); // 1 - decayProgress to go from 0 to 100
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                    skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, blendShapeWeight);
            }
        }
        else
        {
            // Once decay is complete, ensure values are set to 0
            pointLight.range = 0;
            pointLight.intensity = 0;
            sphereCollider.radius = 0.1f;
            
            decayComplete = true;
            OnDecayComplete?.Invoke();

            // Optionally, destroy the GameObject if you want it to disappear
            // Destroy(gameObject);
        }
    }
    
    private void Replenish()
    {
        decayTimer = 0f;

        // Reset light and collider properties to their initial values
        pointLight.range = initialRange;
        pointLight.intensity = initialIntensity;
        sphereCollider.radius = initialRange;

        // Reset blendshape to 0
        if (skinnedMeshRenderers.Count > 0)
        {
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, 0);
        }
        
        decayComplete = false;
    }
}
