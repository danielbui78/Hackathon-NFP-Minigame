using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ballKickClip;

    public void PlayKickSound()
    {
        audioSource.PlayOneShot(ballKickClip);
    }
}
