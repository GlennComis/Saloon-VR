using UnityEngine;

public class SoundInteractable : Interactable
{
   [SerializeField] 
   private AudioClip shotSound;
   private AudioSource audioSource;

   private void Awake()
   {
      audioSource = gameObject.GetComponent<AudioSource>();
      
      if (audioSource == null)
      {
         audioSource = gameObject.AddComponent<AudioSource>();
      }

      // Optional: Set the audio source settings
      audioSource.playOnAwake = false;
   }

   public override void OnShotHit()
   {
      base.OnShotHit();
      OnShotBehaviour();
   }

   protected virtual void OnShotBehaviour()
   {
      Debug.Log("On Shot");
      
      if (shotSound != null)
      {
         if(audioSource.isPlaying)
            audioSource.Stop();
         
         audioSource.PlayOneShot(shotSound);
      }
      else
      {
         Debug.LogWarning("No audio clip assigned to SoundInteractable!", this.gameObject);
      }
   }
}