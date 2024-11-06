using UnityEngine;

public class LockInteractable : DestroyableInteractable
{
    [SerializeField]
    private Door door;

    public override void OnShotHit()
    {
        base.OnShotHit();
        door.OpenDoor();
        Debug.Log("Lock got hit");
    }
}