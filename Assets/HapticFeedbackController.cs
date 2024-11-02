using StudioXRToolkit.Runtime.Scripts.Abstracts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class HapticFeedbackController : SingletonMonoBehaviour<HapticFeedbackController>
{
    [SerializeField]
    private HapticImpulsePlayer leftHandHapticImpulsePlayer;

    [SerializeField]
    private HapticImpulsePlayer rightHandHapticImpulsePlayer;

    public void RightVibration()
    {
        rightHandHapticImpulsePlayer.SendHapticImpulse(1, .2f);
    }
    
    public void LeftVibration()
    {
        leftHandHapticImpulsePlayer.SendHapticImpulse(1, .2f);
    }
}
