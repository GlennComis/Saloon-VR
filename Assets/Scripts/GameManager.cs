using System.Collections;
using System.Collections.Generic;
using StudioXRToolkit.Runtime.Scripts.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public UnityEvent OnNumberLocksCorrect;
    
    [SerializeField] 
    private AudioClip bellPuzzleCorrect;
    [SerializeField] 
    private AudioClip bellPuzzleIncorrect;
    private AudioSource audioSource;


    [SerializeField]
    private TextMeshPro numberOne;
    [SerializeField]
    private TextMeshPro numberTwo;
    [SerializeField]
    private TextMeshPro numberThree;
    private bool didDrink;

    private int expectedValue1, expectedValue2, expectedValue3;
    public NumberKnob numberKnob1, numberKnob2, numberKnob3;
    
    // Reference to the bells in the correct order
    [SerializeField] private List<SoundInteractable> solutionOrder;

    // This will hold the order of bells that the player hits
    private List<SoundInteractable> playerOrder = new List<SoundInteractable>();

    protected override void Awake()
    {
        base.Awake();
        audioSource = gameObject.GetComponent<AudioSource>();
      
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Optional: Set the audio source settings
        audioSource.playOnAwake = false;
        InitNumbers();
    }
    
    // Method called by each bell when it is shot
    public void RegisterBellHit(SoundInteractable bell)
    {
        // Add the bell to the player's sequence
        playerOrder.Add(bell);

        // Check if the player has hit 3 bells
        if (playerOrder.Count == solutionOrder.Count)
        {
            ValidateSequence();
            // Clear the player order for the next attempt
            playerOrder.Clear();
        }
    }

    private void ValidateSequence()
    {
        // Check if player's order matches the solution
        bool isCorrect = true;

        for (int i = 0; i < solutionOrder.Count; i++)
        {
            if (playerOrder[i] != solutionOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        StartCoroutine(ProcessPuzzle(isCorrect));
    }
    
    private IEnumerator ProcessPuzzle(bool isCorrect)
    {
        yield return new WaitForSeconds(2f);
        
        if (bellPuzzleCorrect != null)
        {
            audioSource.Stop();
            var currentAudioClip = isCorrect ? bellPuzzleCorrect : bellPuzzleIncorrect;
            audioSource.PlayOneShot(currentAudioClip);
            numberThree.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No audio clip assigned to SoundInteractable!");
        }
    }

    private void InitNumbers()
    {
        numberOne.gameObject.SetActive(false);
        numberTwo.gameObject.SetActive(false);
        
        expectedValue1 = GetRandomNumber();
        expectedValue2 = GetRandomNumber();
        expectedValue3 = GetRandomNumber();
        
        numberOne.SetText(expectedValue1.ToString());
        numberTwo.SetText(expectedValue2.ToString());
        numberThree.SetText(expectedValue3.ToString());
    }
    
    private int GetRandomNumber()
    {
        return Random.Range(0, 10); // Random.Range for integers is exclusive of the max, so it generates between 0 and 9
    }
    
    public void ValidateNumberKnobs()
    {
        if (numberKnob1.KnobValue == expectedValue1 && numberKnob2.KnobValue == expectedValue2 && numberKnob3.KnobValue == expectedValue3)
        {
            OnNumberLocksCorrect?.Invoke();
            Debug.Log("Escaped!");
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Time's up Motherfucker! Game Over! Darkness reigns etc...");
    }

    public void CompletedDrink()
    {
        if (didDrink) return;
        didDrink = true;
        
        numberOne.gameObject.SetActive(true);
    }
}
