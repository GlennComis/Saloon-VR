using UnityEngine;

public class PointLightTimerCandleColliderHandler : MonoBehaviour
{
    [SerializeField] private PointLightTimer pointLightTimer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            pointLightTimer.Extinguish(true);
        
        if (other.CompareTag("Respawn"))
            pointLightTimer.Replenish();
    }
}
