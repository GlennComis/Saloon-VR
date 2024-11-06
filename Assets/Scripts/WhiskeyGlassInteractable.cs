using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WhiskeyGlassInteractable : MonoBehaviour
{
    // Boolean to check if the player can drink from the glass
    public bool canDrink { get; private set; } = true;

    // Boolean to check if the player is drinking
    public bool isDrinking { get; private set; } = false;

    // Tag to identify the player
    [SerializeField] private string playerTag = "Player";

    // Audio clip to be played when drinking
    [SerializeField] private AudioClip drinkingSound;
    // Audio clip to be played when finished drinking
    [SerializeField] private AudioClip burpSound;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    // Reference to the TiltToBlendshape component
    private TiltToBlendshape tiltToBlendshape;

    private void Awake()
    {
        // Make sure the BoxCollider is set as a trigger
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;

        // Get or add an AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set up the AudioSource
        audioSource.clip = drinkingSound;
        audioSource.loop = true;

        // Get the TiltToBlendshape component
        tiltToBlendshape = GetComponent<TiltToBlendshape>();
    }

    // Called when another collider enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding is the player
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Colliding with player");

            // Check if the glass is not empty and the player is tilting
            UpdateCanDrink(); // Update the canDrink status

            if (tiltToBlendshape != null && canDrink && tiltToBlendshape.isTilting)
            {
                isDrinking = true;
                Debug.Log("Is drinking");
                audioSource.Play(); // Start playing the drinking sound
            }
        }
        else
        {
            Debug.Log("Colliding with other: " + other.gameObject.name);
        }
    }

    // Called when another collider exits this trigger
    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting is the player
        if (other.CompareTag(playerTag))
        {
            StopDrinking(); // Stop drinking logic
        }
    }

    private void Update()
    {
        // Continuously check the currentWeight
        if (tiltToBlendshape != null)
        {
            UpdateCanDrink(); // Update the canDrink status each frame

            if (isDrinking && !tiltToBlendshape.isTilting)
            {
                StopDrinking(); // Stop drinking if the player stops tilting
            }
        }
    }

    private void UpdateCanDrink()
    {
        // Set canDrink to false if the currentWeight is 99.9 or greater
        canDrink = tiltToBlendshape.currentWeight < 99.9f;

        if (!canDrink && audioSource.isPlaying)
        {
            StopDrinking(true);
        }
    }

    private void StopDrinking(bool burp = false)
    {
        if (isDrinking)
        {
            isDrinking = false;
            audioSource.Stop(); // Stop playing the drinking sound
            Debug.Log("Stopped drinking - glass is empty or player stopped tilting."); 
            
            if (burp)
            {
                audioSource.loop = false;
                audioSource.PlayOneShot(burpSound);
                GameManager.Instance.CompletedDrink();
            }
        }
    }
}
