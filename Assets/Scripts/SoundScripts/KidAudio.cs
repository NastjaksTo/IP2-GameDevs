using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rRandom = System.Random;

public class KidAudio : MonoBehaviour
{
    private rRandom randomNumber = new rRandom();       // Random Number.
    private bool isPlayingSounds;                       // Checks if the sound is playing or not.
    private AudioSource[] sounds;                       // Array of AudioClips.

    
    /// <summary>
    /// Gets all audio sources and saves them in the array.
    /// </summary>
    private void Awake()
    {
        sounds = transform.GetComponents<AudioSource>();
    }

    /// <summary>
    /// Plays a random sound of the array.
    /// </summary>
    private void PlaySound()
    {
        if (isPlayingSounds) return;
        int chooseSound = randomNumber.Next(0, 4);
        if (chooseSound == 1 || chooseSound == 2)
        {
            sounds[0].Play();
            StartCoroutine(playingSound());
        } 
        else if (chooseSound == 3)
        {
            sounds[1].Play();
            StartCoroutine(playingSound());
        }
    }

    /// <summary>
    /// Cooldown until the next sound is being played.
    /// </summary>
    /// <returns></returns>
    private IEnumerator playingSound()
    {
        isPlayingSounds = true;
        yield return new WaitForSeconds(7f);
        isPlayingSounds = false;
    }

    /// <summary>
    /// Plays the sounds.
    /// </summary>
    private void Update()
    {
        if (!isPlayingSounds)
        {
            PlaySound();
        }
    }
}
