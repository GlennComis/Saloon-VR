using System;
using System.Collections;
using DG.Tweening;
using StudioXRToolkit.Runtime.Scripts.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
{
    [SerializeField]
    public AudioSource textAudioSource;
    public AudioClip textAudioClip;
    
    [SerializeField]
    public TextMeshProUGUI npcNameLabel;
    [SerializeField]
    public TextMeshProUGUI dialogueTextLabel;
    [SerializeField]
    public AudioSource npcAudioSource;
    
    [SerializeField]
    public ConversationScriptableObject currentConversation;
    private int currentDialogueIndex;
    public event Action OnEndConversation;

    [SerializeField] private GameObject canvasGameObject;
    
    private float dialogueTimer = 0f;
    private float dialogueInterval = 6f; // Interval in seconds
    public bool IsInConversation { get; private set; }

    private void Update()
    {
        if (IsInConversation)
        {
            dialogueTimer += Time.deltaTime; // Increment the timer

            if (dialogueTimer >= dialogueInterval)
            {
                NextDialogue(); // Call the method to proceed to the next dialogue
                dialogueTimer = 0f; // Reset the timer
            }
        }
        else
        {
            dialogueTimer = 0f; 
        }
    }

    public void SetCurrentConversation(ConversationScriptableObject conversationScriptableObject)
    {
        currentConversation = conversationScriptableObject;
    }

    public void StartConversation()
    {
        if (IsInConversation)
        {
            EndConversation();
        }

        canvasGameObject.SetActive(true);
        
        ClearFields();
        currentDialogueIndex = 0;
        IsInConversation = true;
        
        StartCoroutine(SetDialogue(currentDialogueIndex));
    }

    private void NextDialogue()
    {
        if (!IsInConversation) return;
        
        currentDialogueIndex++;
        
        if (currentDialogueIndex > currentConversation.dialogueScriptableObjects.Count - 1)
        {
          EndConversation();
            return;
        }
        
        StartCoroutine(SetDialogue(currentDialogueIndex));
    }

    public void DoEndConversation()
    {
        if (!IsInConversation)
            return;
        
        EndConversation();
    }

    private void EndConversation()
    {
    
        canvasGameObject.SetActive(false);
        currentDialogueIndex = 0;
        IsInConversation = false;

        OnEndConversation?.Invoke();
    }

    private IEnumerator SetDialogue(int dialogueIndex, float textDelay = 0f)
    {
        npcNameLabel.text = currentConversation.dialogueScriptableObjects[dialogueIndex].npcName;
        yield return new WaitForSeconds(textDelay);

        if (currentConversation.dialogueScriptableObjects[dialogueIndex].audioClip != null)
        {
            npcAudioSource.PlayOneShot(currentConversation.dialogueScriptableObjects[dialogueIndex].audioClip);
        }
        
        dialogueTextLabel.text = currentConversation.dialogueScriptableObjects[dialogueIndex].dialogue;
    }

    private void ClearFields()
    {
        npcNameLabel.text = string.Empty;
        dialogueTextLabel.text = string.Empty;
    }

    public void PlayTextAudioClip()
    {
        textAudioSource.PlayOneShot(textAudioClip);
    }
}
