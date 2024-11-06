using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float range = 10f;
    public float shotDelay = 1.5f;                 // Delay between shots
    public AudioClip fireSound;
    public AudioClip jammedSound;                  // Sound that plays when the gun jams
    public AudioClip reloadSound;                  // Sound that plays when reloading
    public ParticleSystem muzzleFlash;
    public Light muzzleFlashLight;
    public float muzzleFlashDuration = .25f;

    [Header("Bullet Settings")]
    public Transform bulletOrigin;
    private AudioSource audioSource;
    private Animator animator;
    private readonly WaitForSeconds timeBetweenShotAndReload = new (.5f);
    private readonly WaitForSeconds reloadTime = new (.7f);
    private float jamChance = 0.05f;                // 5% chance of jamming
    private bool canFire = true;                   // Tracks whether gun can fire
    private int shotsRemaining = 2;                // Allow two shots before reloading
    private static readonly int ReloadAnimatorHash = Animator.StringToHash("Reload");

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void Fire()
    {
        if (!canFire || shotsRemaining <= 0)
            return;

        canFire = false;

        if (Random.value < jamChance)
        {
            if (audioSource != null && jammedSound != null)
                audioSource.PlayOneShot(jammedSound);
            
            Debug.Log("Gun jammed!");
            StartCoroutine(RestoreFireAfterDelay(false));  // Don't reload if jammed
            return;
        }

        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);
        
        HapticFeedbackController.Instance.RightVibration();

        if (muzzleFlash != null)
        {
            muzzleFlash.Stop();
            muzzleFlash.Play();
        }

        MuzzleFlash();

        Vector3 origin = bulletOrigin != null ? bulletOrigin.position : transform.position;
        Vector3 direction = bulletOrigin != null ? bulletOrigin.forward : transform.forward;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (enemy != null)
            {
                Debug.LogWarning("We have hit a enemy, now he needs to die");
                enemy.Die();
            }
            else if (interactable != null)
            {
                Debug.LogWarning("We have hit an interactable");
                interactable.OnShotHit();
            }
            else
                Debug.Log("We have missed the enemy");
        }

        shotsRemaining--;

        // Start the coroutine with the correct boolean for reloading
        StartCoroutine(RestoreFireAfterDelay(shotsRemaining <= 0));
    }

    private void Reload()
    {
        if (animator != null)
        {
            Debug.Log("Playing Reload Animation");
            animator.SetTrigger(ReloadAnimatorHash);

            if (audioSource != null && reloadSound != null)
                audioSource.PlayOneShot(reloadSound);

            shotsRemaining = 2; // Reset shots remaining after reloading
        }
        else
        {
            Debug.LogError("Animator is null");
        }
    }

    private IEnumerator RestoreFireAfterDelay(bool shouldReload)
    {
        if (shouldReload)
        {
            yield return timeBetweenShotAndReload;  
            Reload();
            yield return reloadTime; 
            canFire = true;
        }
        else
        {
            yield return new WaitForSeconds(shotDelay);
            canFire = true;   
        }
    }

    private void MuzzleFlash()
    {
        // Ensure the light starts at intensity 0
        muzzleFlashLight.intensity = 0f;

        // Animate the light's intensity from 0 to 1 and back to 0 once
        muzzleFlashLight.DOIntensity(81.47f, muzzleFlashDuration / 2) // Animate to intensity 1
            .OnComplete(() => muzzleFlashLight.DOIntensity(0f, muzzleFlashDuration / 2)) // Then animate back to 0
            .SetEase(Ease.OutSine); // Smooth easing for the pulse effect
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = bulletOrigin != null ? bulletOrigin.position : transform.position;
        Vector3 direction = bulletOrigin != null ? bulletOrigin.forward : transform.forward;
        Vector3 endPoint = origin + direction * range;

        Gizmos.DrawLine(origin, endPoint);
        Gizmos.DrawWireSphere(endPoint, 0.1f);
    }
}