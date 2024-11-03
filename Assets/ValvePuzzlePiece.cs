using UnityEngine;

public class ValvePuzzlePiece : SoundInteractable
{
    private bool hasTurned;
    private GameObject waterParticles;
    
    protected override void OnShotBehaviour()
    {
        if (hasTurned)
            return;
        
        base.OnShotBehaviour();
        waterParticles.SetActive(false);
        //turn water valve left
    }
}