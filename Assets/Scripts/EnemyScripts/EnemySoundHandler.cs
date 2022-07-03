using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundHandler : MonoBehaviour
{
    private AudioSource audio;
    public static EnemySoundHandler enemySoundhandler;

    [SerializeField]
    AudioClip step;

    [SerializeField]
    AudioClip hit;

    [SerializeField]
    AudioClip hit2;

    [SerializeField]
    AudioClip swoosh;

    [SerializeField]
    AudioClip swoosh2;

    [SerializeField]
    AudioClip death;

    [SerializeField]
    AudioClip scream;

    [SerializeField]
    AudioClip magic;

    /// <summary>
    /// Awake is called before Start
    /// Initializes all Necessary
    /// </summary>
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        enemySoundhandler = this;
    }

    /// <summary>
    /// Plays One Shot of the given AudioClip
    /// </summary>
    private void stepSound()
    {
        audio.PlayOneShot(step, 0.05f);
    }

    /// <summary>
    /// Plays One Shot of the given AudioClip
    /// wich of these two Clips is being played is chosen by a Random Number
    /// </summary>
    public void hitSound()
    {
        int i = Random.Range(1, 3);

        if (i == 1)
        {
            audio.PlayOneShot(hit, 0.2f);
        }
        if (i == 2)
        {
            audio.PlayOneShot(hit, 0.2f);
        }
    }

    /// <summary>
    /// Plays One Shot of the given AudioClip
    /// </summary>
    private void swooshSound()
    {
        audio.PlayOneShot(swoosh, 0.2f);
    }

    /// <summary>
    /// Plays One Shot of the given AudioClip
    /// </summary>
    private void swoosh2Sound()
    {
        audio.PlayOneShot(swoosh2, 0.2f);
    }

    /// <summary>
    /// Plays One Shot of the given AudioClip
    /// </summary>
    private void deathSound()
    {
        audio.PlayOneShot(death, 0.2f);
    }

    /// <summary>
    /// Plays One Shot of the given AudioClip
    /// </summary>
    private void screamSound()
    {
        audio.PlayOneShot(scream, 0.2f);
    }

    /// <summary>
    /// Plays One Shot of the given AudioClip
    /// </summary>
    private void magicSound()
    {
        audio.PlayOneShot(magic, 0.2f);
    }
}
