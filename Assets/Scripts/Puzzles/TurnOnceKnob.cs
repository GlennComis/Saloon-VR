using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Events;

public class TurnOnceKnob : MonoBehaviour
{
    public UnityEvent OnKnobTurnedFully;
    
    [Header("Depedencies")]
    [SerializeField] private XRKnob knob;

    private bool fullyTurned;

    public void CheckKnobValue()
    {
        if (knob.value >= 0.99f && !fullyTurned)
        {
            // knob.value = 1f; // Produces strange errors in XRKnob.cs
            knob.enabled = false;
            fullyTurned = true;
            OnKnobTurnedFully?.Invoke();
        }
    }

    public void ResetKnob()
    {
        knob.value = 0f;
        knob.enabled = true;
        fullyTurned = false;
    }
}
