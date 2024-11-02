using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public float range = 10f;
    public AudioClip fireSound;
    public AudioClip jammedSound;                  // Sound that plays when the gun jams
    public ParticleSystem muzzleFlash;
    public bool infiniteShots = false;

    [Header("Bullet Settings")]
    public Transform bulletOrigin;

    [Header("Debugging")]
    public bool showLineRendererDebug = false;     // Checkbox for showing LineRenderer in the editor

    private bool canFire = true;
    private AudioSource audioSource;
    private LineRenderer lineRenderer;
    private float jamChance = 0.2f;                // 20% chance of jamming

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        // Configure the LineRenderer
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false; 
    }

    public void Fire()
    {
        // Check if the gun can fire, unless infiniteShots is enabled
        if (!canFire && !infiniteShots)
            return;

        // Handle gun jamming with a 20% chance
        if (Random.value < jamChance)
        {
            if (audioSource != null && jammedSound != null)
            {
                audioSource.PlayOneShot(jammedSound);
            }
            Debug.LogWarning("Gun jammed!");
            return; // Exit early since the gun has jammed
        }

        canFire = false;

        // Play the firing sound
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // Play the muzzle flash particle effect
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Set the origin position and direction for the raycast
        Vector3 origin = bulletOrigin != null ? bulletOrigin.position : transform.position;
        Vector3 direction = bulletOrigin != null ? bulletOrigin.forward : transform.forward;

        // Cast a ray from the bullet origin in the bulletOrigin's forward direction
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        // Enable and set up the LineRenderer to visualize the shot
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, origin);

        if (Physics.Raycast(ray, out hit, range))
        {
            // Set the end position of the LineRenderer at the hit point
            lineRenderer.SetPosition(1, hit.point);

            PlayerController player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.LogWarning("We have hit a player, now he needs to die");
            }
            else
            {
                Debug.Log("We have missed the player");
            }
        }
        else
        {
            // If we don't hit anything, set the end point of the LineRenderer at max range
            lineRenderer.SetPosition(1, origin + direction * range);
        }

        // Start a coroutine to temporarily display the line
        StartCoroutine(DisableLineRendererAfterTime(0.5f)); // Increased duration to 0.5 seconds
    }

    private System.Collections.IEnumerator DisableLineRendererAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.enabled = false;
    }

    public void ResetGun()
    {
        canFire = true;

        // Disable the LineRenderer when the gun is reset
        lineRenderer.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a line representing the range and direction of the gun's shot
        Gizmos.color = Color.red;

        // Start point for the gizmo line (use bulletOrigin if set, otherwise gun's position)
        Vector3 origin = bulletOrigin != null ? bulletOrigin.position : transform.position;
        Vector3 direction = bulletOrigin != null ? bulletOrigin.forward : transform.forward;

        // Calculate end point based on range and forward direction
        Vector3 endPoint = origin + direction * range;

        // Draw the line
        Gizmos.DrawLine(origin, endPoint);

        // Draw a sphere at the end point to indicate the max range
        Gizmos.DrawWireSphere(endPoint, 0.1f);

        // Show the LineRenderer for debugging if the checkbox is checked
        if (showLineRendererDebug)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, endPoint);
        }
    }
}
