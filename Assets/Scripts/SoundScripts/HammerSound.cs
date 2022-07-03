using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSound : MonoBehaviour
{
    private AudioSource hammersound;

    private void Awake()
    {
        hammersound = transform.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            hammersound.Play();
        }
    }
}
