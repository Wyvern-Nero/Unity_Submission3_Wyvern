using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource collectTargetSound;
    [SerializeField] private AudioSource collectSecretSounds;

    public void PlayDeathSound()
    {
        deathSound.Play();
    }

    public void PlayJumpSound()
    {
        jumpSound.Play();
    }
    
    public void PlayTargetSound()
    {
        collectTargetSound.Play();
    }
    
    public void PlaySecretSound()
    {
        collectSecretSounds.Play();
    }
}
