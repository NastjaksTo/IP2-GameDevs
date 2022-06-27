using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rRandom = System.Random;

public class KidAudio : MonoBehaviour
{
    private rRandom randomNumber = new rRandom();

    private bool isPlayingSounds;
    
    private AudioSource[] sounds;

    

    private void Awake()
    {
        sounds = transform.GetComponents<AudioSource>();
    }

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

    private IEnumerator playingSound()
    {
        isPlayingSounds = true;
        yield return new WaitForSeconds(7f);
        isPlayingSounds = false;
    }

    private void Update()
    {
        if (!isPlayingSounds)
        {
            PlaySound();
        }
    }
}
