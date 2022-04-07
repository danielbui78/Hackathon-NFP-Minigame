using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ballKickClip;
    [SerializeField] private AudioClip gruntClip;
    [SerializeField] private AudioClip goalClip;
    [SerializeField] private AudioClip blockClip;
    [SerializeField] private AudioClip winnerClip;
    [SerializeField] private AudioClip looserClip;

    public void PlayKickSound()
    {
        if(gruntClip != null) audioSource.PlayOneShot(gruntClip, 0.2f);
        audioSource.PlayOneShot(ballKickClip);
    }

    public void PlayGoalSound()
    {
        audioSource.PlayOneShot(goalClip);
    }

    public void PlayBlockSound()
    {
        audioSource.PlayOneShot(blockClip, 0.2f);
    }

    public void PlayWinnerSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(winnerClip, 0.4f);
    }

    public void PlayLooserSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(looserClip, 0.2f);
    }
}
