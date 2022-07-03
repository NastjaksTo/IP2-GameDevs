using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using StarterAssets;
using UnityEngine;

public class DebuffOne : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
}
