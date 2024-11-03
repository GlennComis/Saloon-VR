using TMPro;
using Unity.VRTemplate;
using UnityEngine;

public class NumberKnob : MonoBehaviour
{
    [Header("Depedencies")]
    [SerializeField] private XRKnob knob;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private AudioSource audioSource;
    
    public int KnobValue { get; private set; }

    private int previousValue;

    public void SetKnobValue()
    {
        // 9 positions on the knob, no looping around
        int knobPosition = Mathf.RoundToInt(knob.value * 9);
        valueText.text = knobPosition.ToString();
        KnobValue = knobPosition;
        
        if (!audioSource.isPlaying && KnobValue != previousValue)
            audioSource.Play();
        
        previousValue = KnobValue;
    }
}
