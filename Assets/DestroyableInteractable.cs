using System.Collections;
using UnityEngine;

public class DestroyableInteractable : SoundInteractable
{
    private MeshRenderer meshRenderer;

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void OnShotHit()
    {
        base.OnShotHit();

        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // Disable the mesh renderer
        }

        StartCoroutine(DestroyAfterSound());
    }

    private IEnumerator DestroyAfterSound()
    {
        if (audioSource != null && shotSound != null)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
        gameObject.SetActive(false);
    }
}