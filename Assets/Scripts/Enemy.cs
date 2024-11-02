using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 5f;  // Movement speed of the enemy
    
    // State
    private Vector3 randomDirection;

    private void Start()
    {
        // Generate a random direction
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        // Set the random direction and normalize it to ensure consistent speed
        randomDirection = new Vector3(randomX, randomY, randomZ).normalized;

        // Schedule destruction after 5 seconds
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        // Move the enemy in the random direction
        transform.Translate(randomDirection * (speed * Time.deltaTime));
    }
}
