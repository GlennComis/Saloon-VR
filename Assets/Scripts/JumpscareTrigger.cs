using System.Collections;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip jumpscareClip;   // The audio clip to play
    [SerializeField] private float cooldown = 5f;       // Cooldown time in seconds
    [SerializeField] private bool oneShot = true;       // If true, the jumpscare will only trigger once
    private bool hasTriggered = false;                  // Keeps track if the jumpscare has been triggered
    private bool onCooldown = false;                    // Keeps track of cooldown status

    private AudioSource audioSource;

    private void Start()
    {
        // Ensure we have an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = jumpscareClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered and conditions are met
        if (other.CompareTag("Player") && !onCooldown && (!oneShot || !hasTriggered))
        {
            PlayJumpscare();
        }
    }

    private void PlayJumpscare()
    {
        // Play the jumpscare audio and set flags
        audioSource.Play();
        hasTriggered = true;
        onCooldown = true;

        // Start the cooldown coroutine
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(audioSource.clip.length + cooldown); // Wait for clip duration + cooldown
        onCooldown = false; // Reset cooldown
    }
}