using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool dialogueEndTrigger;
    
    [SerializeField]
    private ConversationScriptableObject conversationScriptableObject;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            if (dialogueEndTrigger)
            {
                DialogueManager.Instance.DoEndConversation();
                return;
            }
            
            DialogueManager.Instance.StartConversation();
        }
        else
        {
            Debug.Log(other.gameObject.name, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            DialogueManager.Instance.DoEndConversation();
        }
    }
}
