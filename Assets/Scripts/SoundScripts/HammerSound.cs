using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSound : MonoBehaviour
{
    private AudioSource hammersound;        // Reference to the AudioSource.

    /// <summary>
    /// Gets the AudioSource.
    /// </summary>
    private void Awake()
    {
        hammersound = transform.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays the audiosource when colliding with any gameobject with the layer number 3 (Enviroment).
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            hammersound.Play();
        }
    }
}
