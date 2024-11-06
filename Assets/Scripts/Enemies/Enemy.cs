using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = .5f;  // Movement speed of the enemy
    public float rotationSpeed = 4f; // Speed of the rotation

    [SerializeField]
    private List<GameObject> masks;
    
    // State
    private Vector3 randomDirection;
    
    private Transform playerTransform;

    private void OnEnable()
    {
        foreach (var mask in masks)
        {
            mask.SetActive(false);
        }
        
        int randomIndex = Random.Range(0, masks.Count);
        
        masks[randomIndex].SetActive(true);
    }
    
    private void Start()
    {
        playerTransform = PlayerController.Instance.headTransform;
    }

    private void Update()
    {
        // Check if the player transform is assigned
        if (playerTransform != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = (playerTransform.position + - transform.position).normalized;

            // Move the enemy towards the player
            transform.position += direction * speed * Time.deltaTime;
            
            // Calculate the target rotation to face the player
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
