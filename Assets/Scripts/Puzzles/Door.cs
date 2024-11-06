using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float targetRotationZ;
    public float duration = 1f;
    
    public void OpenDoor()
    {
        transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetRotationZ), duration);
    }
}