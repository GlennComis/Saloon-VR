using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Igniteable : MonoBehaviour
{
    public UnityEvent OnIgnited;
    
    [Header("Dependencies")]
    [SerializeField] private Light pointLight;
    [SerializeField] private GameObject fireParticle;
    
    
    [Header("Configuration")]
    [SerializeField] private float targetIntensity = 0.2f;
    [SerializeField] private float ignitionSpeed = 1f;
    
    // State
    private bool ignited;
    
    public bool Ignited => ignited;
    
    public void Ignite()
    {
        if (ignited)
            return;
        
        pointLight.DOIntensity(targetIntensity, ignitionSpeed);
        ignited = true;
        fireParticle.SetActive(true);
        OnIgnited?.Invoke();
    }

    public void Reset()
    {
        ignited = false;
        pointLight.intensity = 0f;
    }
}