using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PointLightTimer : MonoBehaviour
{
    public UnityEvent<bool> OnFlameExtinguished;
    
    [Header("Dependencies")]
    [SerializeField] private Light pointLight;
    [SerializeField] private SphereCollider sphereCollider;
    //[SerializeField] private GameObject candleParticle;
    
    [Header("Optional Blendshape")]
    [SerializeField] private List<SkinnedMeshRenderer> skinnedMeshRenderers; // The renderer with the blendshape
    
    [Header("Configuration")]
    [SerializeField] private float decayDuration = 5f; // Time in seconds for the decay to complete
    [SerializeField] private float initialRange = 3f; // Starting range for both collider and light
    [SerializeField] private float initialIntensity = 0.5f; // Starting intensity for the light
    [SerializeField] private Ease lightDecayEase = Ease.OutCirc; // Easing function for light decay
    private const int BLEND_SHAPE_INDEX = 0; // Index of the blendshape to modify

    // State
    private float decayTimer = 0f;
    private bool decayComplete;
    
    public bool DecayComplete => decayComplete;
    
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
            float decayProgressLinear = 1f - (decayTimer / decayDuration);
            float decayProgress = DOVirtual.EasedValue(0f, 1f, decayProgressLinear, lightDecayEase);

            if (float.IsNaN(decayProgress)) // Ensure the decay progress is a valid number (prevents errors at very last frame).
                return;

            // Apply decay to range and intensity
            pointLight.range = initialRange * decayProgress;
            pointLight.intensity = initialIntensity * decayProgress;
            sphereCollider.radius = initialRange * decayProgress;
            
            // Update blendshape weight from 0 to 100 based on decay progress
            if (skinnedMeshRenderers.Count > 0)
            {
                float blendShapeWeight = Mathf.Lerp(0, 100, 1f - decayProgress); // 1 - decayProgress to go from 0 to 100
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                    skinnedMeshRenderer.SetBlendShapeWeight(BLEND_SHAPE_INDEX, blendShapeWeight);
            }
        }
        else
        {
            // Once decay is complete, ensure values are set to 0
            Extinguish();
        }
    }

    public void Extinguish(bool extinguishedByWater = false)
    {
        pointLight.range = 0;
        pointLight.intensity = 0;
        sphereCollider.radius = 0.1f;
            
        //candleParticle.SetActive(false);
        decayComplete = true;
        OnFlameExtinguished?.Invoke(extinguishedByWater);
    }

    public void Replenish()
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
                skinnedMeshRenderer.SetBlendShapeWeight(BLEND_SHAPE_INDEX, 0);
        }
        
        //candleParticle.SetActive(true);
        decayComplete = false;
    }
}
