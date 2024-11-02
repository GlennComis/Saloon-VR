using UnityEngine;
using UnityEngine.Events;

public class InsanityController : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] private float maxInsanity = 40f; // Insanity cap
    [SerializeField] private float brightnessDecayRate = 5f;
    [SerializeField] private float insanityDecayRate = 1f;
    [SerializeField] private float maxBrightness = 100f;
    [SerializeField] private AnimationCurve insanityCurve; // Animation curve to define insanity increase based on brightness. X-axis: brightness, Y-axis: insanity rate

    // State
    private bool inLightRange = false;
    private float currentBrightness = 100f;
    private float currentInsanity = 0f;
    private float minBrightness = 0f;
    
    public float CurrentInsanity => currentInsanity;
    public float MaxInsanity => maxInsanity;

    void Start()
    {
        // Ensure the curve is set up properly
        if (insanityCurve == null || insanityCurve.keys.Length == 0)
        {
            // Set a default linear curve if none is provided
            insanityCurve = AnimationCurve.Linear(0, 1, maxBrightness, 0);
        }
    }

    void Update()
    {
        if (inLightRange)
        {
            currentBrightness = Mathf.Min(maxBrightness, currentBrightness + brightnessDecayRate * Time.deltaTime);
            if (currentBrightness > minBrightness) // If the brightness is above the minimum, decrease insanity at insanityDecayRate (constant).
            {
                currentInsanity = Mathf.Max(0, currentInsanity - insanityDecayRate * Time.deltaTime);
            }
        }
        else
        {
            // Decrease brightness over time
            currentBrightness = Mathf.Max(minBrightness, currentBrightness - brightnessDecayRate * Time.deltaTime);

            if (currentInsanity >= maxInsanity)
                return;
            
            // Get insanity increase rate from the curve based on the current brightness level
            float insanityIncreaseRate = insanityCurve.Evaluate(currentBrightness);
            currentInsanity += insanityIncreaseRate * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            inLightRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            inLightRange = false;
        }
    }
}
