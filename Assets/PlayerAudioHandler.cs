using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip startWalk;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landingSound;
    [SerializeField] AudioClip checkpointHit;
    [SerializeField] AudioClip coinCollect;

    public void PlayDeathSound(){
        audioSource.clip = deathSound;
        audioSource.Play();
    }
    public void PlayDamageSound()
    {
        audioSource.clip = damageSound;
        audioSource.Play();
    }

    
    public void PlayStartWalkSound()
    {
        audioSource.clip = startWalk;
        audioSource.Play();
    }

    
    public void PlayJumpSound()
    {
        audioSource.clip = jumpSound;
        audioSource.Play();
    }

    
    public void PlayLandingSound()
    {
        audioSource.clip = landingSound;
        audioSource.Play();
    }

    
    public void PlayCheckpointSound()
    {
        audioSource.clip = checkpointHit;
        audioSource.Play();
    }

    
    public void PlayCoinCollectSound()
    {
        audioSource.clip = coinCollect;
        audioSource.Play();
    }

}
