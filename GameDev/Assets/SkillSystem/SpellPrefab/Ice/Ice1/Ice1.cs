using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice1 : MonoBehaviour
{
    public Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
    }
}
