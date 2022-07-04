using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using StarterAssets;
using UnityEngine;

public class DebuffOne : MonoBehaviour
{
    /// <summary>
    /// Destroys the gameobject after it gets instantiated in 5 seconds.
    /// </summary>
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
}
